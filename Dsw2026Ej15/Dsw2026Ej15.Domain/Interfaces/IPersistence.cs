using System;
using System.Collections.Generic;
using System.Text;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Speciality? GetSpecialityById(Guid id);
        void SaveDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        List<Doctor> GetActiveDoctors();
        Doctor? GetDoctorById(Guid id);
        void ToggleDoctorActive(Guid id);
    }
}