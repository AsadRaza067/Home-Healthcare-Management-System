using Dapper;
using HomeHealthcare.API.Data;
using HomeHealthcare.API.Models;
using HomeHealthcare.API.Repositories.Interfaces;

namespace HomeHealthcare.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { UserId = userId });
        }

        public async Task<int> CreateAsync(User user)
        {
            var query = @"INSERT INTO Users (FullName, Email, PasswordHash, Role, CreatedAt)
                          OUTPUT INSERTED.UserId
                          VALUES (@FullName, @Email, @PasswordHash, @Role, @CreatedAt)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, user);
        }
    }
}
