using HomeHealthcare.API.Models;

namespace HomeHealthcare.API.Repositories.Interfaces
{
    public interface ICarePlanRepository
    {
        Task<IEnumerable<CarePlan>> GetAllAsync();
        Task<IEnumerable<CarePlan>> GetByPatientIdAsync(int patientId);
        Task<CarePlan?> GetByIdAsync(int id);
        Task<int> CreateAsync(CarePlan carePlan);
        Task<bool> UpdateAsync(CarePlan carePlan);
        Task<bool> DeleteAsync(int id);
    }
}
