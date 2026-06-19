using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; init; }
        public string LicenseNumber { get; init; }
        public Speciality? Speciality { get; set; }
        public bool IsActive { get; set; }

        public Guid SpecialityId { get; set; }

        public Doctor(string name, string licenseNumber, Speciality speciality)
        {
            Name = name;
            LicenseNumber = licenseNumber;
            Speciality = speciality;
            IsActive = true;
        }
    }
}