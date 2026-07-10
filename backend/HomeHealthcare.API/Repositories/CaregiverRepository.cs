using Dapper;
using HomeHealthcare.API.Data;
using HomeHealthcare.API.Models;
using HomeHealthcare.API.Repositories.Interfaces;

namespace HomeHealthcare.API.Repositories
{
    public class CaregiverRepository : ICaregiverRepository
    {
        private readonly DapperContext _context;

        public CaregiverRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Caregiver>> GetAllAsync()
        {
            var query = "SELECT * FROM Caregivers ORDER BY CreatedAt DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Caregiver>(query);
        }

        public async Task<Caregiver?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Caregivers WHERE CaregiverId = @CaregiverId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Caregiver>(query, new { CaregiverId = id });
        }

        public async Task<Caregiver?> GetByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM Caregivers WHERE UserId = @UserId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Caregiver>(query, new { UserId = userId });
        }

        public async Task<int> CreateAsync(Caregiver caregiver)
        {
            var query = @"INSERT INTO Caregivers (UserId, FullName, Email, Phone, Specialization, IsAvailable, CreatedAt)
                          OUTPUT INSERTED.CaregiverId
                          VALUES (@UserId, @FullName, @Email, @Phone, @Specialization, @IsAvailable, @CreatedAt)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, caregiver);
        }

        public async Task<bool> UpdateAsync(Caregiver caregiver)
        {
            var query = @"UPDATE Caregivers SET FullName=@FullName, Phone=@Phone,
                          Specialization=@Specialization, IsAvailable=@IsAvailable WHERE CaregiverId=@CaregiverId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, caregiver);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Caregivers WHERE CaregiverId = @CaregiverId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { CaregiverId = id });
            return rows > 0;
        }
    }
}
