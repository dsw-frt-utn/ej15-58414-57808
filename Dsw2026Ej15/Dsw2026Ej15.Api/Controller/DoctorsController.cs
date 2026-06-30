using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Dsw2026Ej15.Domain.Exceptions;

namespace Dsw2026Ej15.Api.Controller;

public class DoctorsController : AppController
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("Nombre y matricula son requeridos");
        }

        var speciality = await _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality is null)
        {
            throw new ValidationException("Especialidad no Existe");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        await _persistence.SaveDoctor(doctor);
        return Created();
    }

    [HttpGet()]
    public async Task<IActionResult> GetActiveDoctors()
    {
        var activeDoctors = await _persistence.GetActiveDoctors();
        return Ok(activeDoctors.Select(d => new DoctorModel.Response(d.Id, d.Name, d.LicenseNumber, d.Speciality?.Name)));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var doctor = await _persistence.GetActiveDoctorById(id);
        if(doctor is null)
        {
            throw new ValidationException("El medico no existe o no esta activo.");
        }

        var response = new DoctorModel.Response(doctor.Id, doctor.Name, doctor.LicenseNumber, doctor.Speciality?.Name);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDoctor(Guid id)
    {
        var doctor = await _persistence.GetActiveDoctorById(id);
        if(doctor is null)
        {
            throw new ValidationException("El medico no existe o no esta activo.");
        }

        doctor.Deactivate();
        await _persistence.UpdateDoctor(doctor);
        return NoContent();
    }
}