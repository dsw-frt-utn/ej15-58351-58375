using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private List<Doctor> _doctors = [];
        private List<Speciality> _specialities = [];


        public PersistenceInMemory()
        {
            LoadSpecialities();
        }

        public Speciality? GetSpecialityById(Guid id)
        {
            return _specialities.FirstOrDefault(s => s.Id == id);
        }

        public void SaveDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);
        }
        public List<Doctor> GetAllDoctors()
        {
            foreach (var doctor in _doctors)
            {
                doctor.Speciality = GetSpecialityById(doctor.SpecialityId);
            }
            return _doctors.ToList();
        }

        public Doctor? GetDoctorById(Guid id)
        {
            var doctor = _doctors.FirstOrDefault(d => d.Id == id);
            if (doctor != null)
            {
                doctor.Speciality = GetSpecialityById(doctor.SpecialityId);
            }
            return doctor;
        }
        private void LoadSpecialities()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Sources", "specialities.json");

                var json = File.ReadAllText(jsonPath);
                var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? [];
                _specialities = [.. specialities.Select(s => new Speciality(s.Name, s.Description, s.Id))];
            }
            catch (Exception)
            {
            }
        }
        public void ToggleDoctorActive(Guid id)
        {
            var doctor = GetDoctorById(id);
            if (doctor != null)
            {
                doctor.IsActive = !doctor.IsActive;
            }
        }
    }
}