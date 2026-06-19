using Dsw2026Ej15.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Data;
using System.ComponentModel.DataAnnotations;
using Dsw2026Ej15.Domain.Interfaces;

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
            throw new ValidationException("Nombre y Matricula son requeridos");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecializationId);
        if (speciality is null)
        {
            throw new ValidationException("Especialidad no encontrada");
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

    [HttpGet("doctors/{id}")]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {

        var doctor = _persistence.GetDoctorById(id);


        if (doctor is null || !doctor.IsActive)
        {
            throw new ValidationException($"No se encontró un médico activo con el ID {id}");
        }


        return Ok(new
        {
            doctor.Name,
            doctor.LicenseNumber,
            SpecialityName = doctor.Speciality?.Name ?? "Sin especialidad"
        });
    }
    [HttpDelete("doctors/{id}")]
    public async Task<IActionResult> DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor is null || !doctor.IsActive)
        {
            throw new ValidationException($"No se encontró un médico activo con el ID {id}");
        }

        _persistence.ToggleDoctorActive(id);

        return NoContent();
    }
}