using HomeHealthcare.API.Models;

namespace HomeHealthcare.API.Repositories.Interfaces
{
    public interface ICaregiverRepository
    {
        Task<IEnumerable<Caregiver>> GetAllAsync();
        Task<Caregiver?> GetByIdAsync(int id);
        Task<Caregiver?> GetByUserIdAsync(int userId);
        Task<int> CreateAsync(Caregiver caregiver);
        Task<bool> UpdateAsync(Caregiver caregiver);
        Task<bool> DeleteAsync(int id);
    }
}
