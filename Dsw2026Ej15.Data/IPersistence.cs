namespace Dsw2026Ej15.Data;

using System;
using System.Collections.Generic;
using Dsw2026Ej15.Domain;

{
    public interface IPersistence
    {
        Speciality GetSpecialityById(Guid id);
        void AddDoctor(Doctor doctor);
        IEnumerable<Doctor> GetActiveDoctors();
        Doctor GetDoctorById(Guid id);
        void UpdateDoctor(Doctor doctor);
    }
}