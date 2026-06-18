namespace Dsw2026Ej15.Api.Controllers;

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Data;

{
    [ApiController]
    [Route("api/[controller]")] 
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [cite_start]
        [cite_start]
       
        [HttpPost]
        public IActionResult CreateDoctor([FromBody] CreateDoctorRequest request)
        {
            [cite_start]
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("Name es requerido."); 
            
            if (string.IsNullOrWhiteSpace(request.LicenseNumber))
                throw new ValidationException("LicenseNumber es requerido."); 

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality == null)
                throw new ValidationException("La especialidad (SpecialityId) no existe."); 

            [cite_start]
            var newDoctor = new Doctor
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                LicenseNumber = request.LicenseNumber,
                Speciality = speciality,
                IsActive = true
            };

            _persistence.AddDoctor(newDoctor);

            [cite_start]
            return Created($"/api/doctors/{newDoctor.Id}", newDoctor);
        }

        [cite_start]
        [cite_start]
        [HttpGet]
        public IActionResult GetActiveDoctors()
        {
            var doctors = _persistence.GetActiveDoctors();
            
            [cite_start]
            return Ok(doctors);
        }

        
        [cite_start]
        [cite_start]
        [HttpGet("{id:guid}")]
        public IActionResult GetDoctorById(Guid id)
        {
            var doctor = _persistence.GetDoctorById(id);

            [cite_start]
            if (doctor == null || !doctor.IsActive)
                return NotFound("Médico no encontrado o no está activo."); 

            [cite_start]
            var response = new DoctorResponse
            {
                Name = doctor.Name,
                LicenseNumber = doctor.LicenseNumber,
                SpecialityName = doctor.Speciality?.Name
            };

            return Ok(response);
        }

        [cite_start]
        [cite_start]
        
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteDoctor(Guid id)
        {
            var doctor = _persistence.GetDoctorById(id);

            [cite_start]
            if (doctor == null || !doctor.IsActive)
                return NotFound("Médico no encontrado o no está activo."); 

           
            doctor.IsActive = false;
            _persistence.UpdateDoctor(doctor);

            [cite_start]
            return NoContent();
        }
    }

    
    
    [cite_start]
    public class CreateDoctorRequest
    {
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public Guid SpecialityId { get; set; }
    }

    [cite_start]
    public class DoctorResponse
    {
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public string SpecialityName { get; set; }
    }
}