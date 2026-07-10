using HomeHealthcare.API.Models;

namespace HomeHealthcare.API.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetByCaregiverIdAsync(int caregiverId);
        Task<Appointment?> GetByIdAsync(int id);
        Task<bool> HasConflictAsync(int caregiverId, DateTime scheduledDate, string timeSlot);
        Task<int> CreateAsync(Appointment appointment);
        Task<bool> UpdateStatusAsync(int id, string status, string visitNotes);
        Task<bool> DeleteAsync(int id);
    }
}
