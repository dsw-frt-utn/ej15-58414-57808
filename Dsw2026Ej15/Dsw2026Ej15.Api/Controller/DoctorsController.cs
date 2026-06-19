using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain;
using System;
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dsw2026Ej15.Api.Controller;

[ApiController]
[Route("api/doctors")]


public class DoctorsController : AppController
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        this._persistence = persistence;
    }

    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            return BadRequest("Nombre y matricula son requeridos");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality is null)
        {
            return BadRequest("Especialidad no Existe");
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
    

}