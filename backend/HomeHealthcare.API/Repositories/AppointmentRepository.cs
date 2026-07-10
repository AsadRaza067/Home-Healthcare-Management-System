using Dapper;
using HomeHealthcare.API.Data;
using HomeHealthcare.API.Models;
using HomeHealthcare.API.Repositories.Interfaces;

namespace HomeHealthcare.API.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DapperContext _context;

        public AppointmentRepository(DapperContext context)
        {
            _context = context;
        }

        private const string BaseSelect = @"
            SELECT a.*, p.FullName AS PatientName, c.FullName AS CaregiverName
            FROM Appointments a
            INNER JOIN Patients p ON a.PatientId = p.PatientId
            INNER JOIN Caregivers c ON a.CaregiverId = c.CaregiverId";

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            var query = BaseSelect + " ORDER BY a.ScheduledDate DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Appointment>(query);
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            var query = BaseSelect + " WHERE a.PatientId = @PatientId ORDER BY a.ScheduledDate DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Appointment>(query, new { PatientId = patientId });
        }

        public async Task<IEnumerable<Appointment>> GetByCaregiverIdAsync(int caregiverId)
        {
            var query = BaseSelect + " WHERE a.CaregiverId = @CaregiverId ORDER BY a.ScheduledDate DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Appointment>(query, new { CaregiverId = caregiverId });
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            var query = BaseSelect + " WHERE a.AppointmentId = @AppointmentId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Appointment>(query, new { AppointmentId = id });
        }

        public async Task<bool> HasConflictAsync(int caregiverId, DateTime scheduledDate, string timeSlot)
        {
            var query = @"SELECT COUNT(1) FROM Appointments
                          WHERE CaregiverId = @CaregiverId AND ScheduledDate = @ScheduledDate
                          AND TimeSlot = @TimeSlot AND Status != 'Cancelled'";
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(query, new { CaregiverId = caregiverId, ScheduledDate = scheduledDate, TimeSlot = timeSlot });
            return count > 0;
        }

        public async Task<int> CreateAsync(Appointment appointment)
        {
            var query = @"INSERT INTO Appointments (PatientId, CaregiverId, ScheduledDate, TimeSlot, Status, VisitNotes, CreatedAt)
                          OUTPUT INSERTED.AppointmentId
                          VALUES (@PatientId, @CaregiverId, @ScheduledDate, @TimeSlot, @Status, @VisitNotes, @CreatedAt)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, appointment);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status, string visitNotes)
        {
            var query = @"UPDATE Appointments SET Status = @Status, VisitNotes = @VisitNotes WHERE AppointmentId = @AppointmentId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { AppointmentId = id, Status = status, VisitNotes = visitNotes });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Appointments WHERE AppointmentId = @AppointmentId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { AppointmentId = id });
            return rows > 0;
        }
    }
}
