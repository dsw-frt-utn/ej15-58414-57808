using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain;
using System;
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

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("Nombre y matricula son requeridos");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality is null)
        {
            throw new ValidationException("Especialidad no Existe");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        _persistence.SaveDoctor(doctor);
        return Created();
    }

    [HttpGet]
    public IActionResult GetActiveDoctors()
    {
        var ActiveDoctors = _persistence.GetActiveDoctors();
        return Ok(ActiveDoctors);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetActiveDoctorById(id);
        if(doctor is null)
        {
            throw new ValidationException("El medico no existe o no esta activo.");
        }

        var response = new DoctorModel.Response(doctor.Id, doctor.Name, doctor.LicenseNumber, doctor.Speciality.Name);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetActiveDoctorById(id);
        if(doctor is null)
        {
            throw new ValidationException("El medico no existe o no esta activo.");
        }

        doctor.Deactivate();
        return NoContent();
    }
}