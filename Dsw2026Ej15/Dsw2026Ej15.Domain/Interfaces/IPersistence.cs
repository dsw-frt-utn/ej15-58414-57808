using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityById(Guid id);
        Task SaveDoctor(Doctor doctor);
        Task<IEnumerable<Doctor>> GetActiveDoctors();
        Task<Doctor?> GetActiveDoctorById(Guid id);
        Task UpdateDoctor(Doctor doctor);
    }
}