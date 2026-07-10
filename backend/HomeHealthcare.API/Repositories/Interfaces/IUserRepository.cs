using HomeHealthcare.API.Models;

namespace HomeHealthcare.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<int> CreateAsync(User user);
    }
}
