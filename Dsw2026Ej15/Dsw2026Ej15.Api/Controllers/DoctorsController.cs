using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Dsw2026Ej15.Api.Controllers;

public class DoctorsController : AppController
{
    private readonly IPersistence _persistence;
    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (String.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            return BadRequest("Nombre y Matricula son requeridos");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecializationId);
        if (speciality is null)
        {
            return BadRequest("Especialidad no existente");
        }
        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        _persistence.SaveDoctor(doctor);

        return Created();
    }

    [HttpGet("doctors")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = _persistence.GetAllDoctors();

        var activeDoctors = doctors.Where(d => d.IsActive).ToList();

        return Ok(activeDoctors);
    }
}