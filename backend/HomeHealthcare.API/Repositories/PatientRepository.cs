using Dapper;
using HomeHealthcare.API.Data;
using HomeHealthcare.API.Models;
using HomeHealthcare.API.Repositories.Interfaces;

namespace HomeHealthcare.API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DapperContext _context;

        public PatientRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            var query = "SELECT * FROM Patients ORDER BY CreatedAt DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Patient>(query);
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Patients WHERE PatientId = @PatientId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Patient>(query, new { PatientId = id });
        }

        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM Patients WHERE UserId = @UserId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Patient>(query, new { UserId = userId });
        }

        public async Task<int> CreateAsync(Patient patient)
        {
            var query = @"INSERT INTO Patients (UserId, FullName, Email, Phone, Address, DateOfBirth, MedicalHistory, CreatedAt)
                          OUTPUT INSERTED.PatientId
                          VALUES (@UserId, @FullName, @Email, @Phone, @Address, @DateOfBirth, @MedicalHistory, @CreatedAt)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, patient);
        }

        public async Task<bool> UpdateAsync(Patient patient)
        {
            var query = @"UPDATE Patients SET FullName=@FullName, Phone=@Phone, Address=@Address,
                          DateOfBirth=@DateOfBirth, MedicalHistory=@MedicalHistory WHERE PatientId=@PatientId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, patient);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Patients WHERE PatientId = @PatientId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { PatientId = id });
            return rows > 0;
        }
    }
}
