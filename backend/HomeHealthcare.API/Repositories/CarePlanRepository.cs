using Dapper;
using HomeHealthcare.API.Data;
using HomeHealthcare.API.Models;
using HomeHealthcare.API.Repositories.Interfaces;

namespace HomeHealthcare.API.Repositories
{
    public class CarePlanRepository : ICarePlanRepository
    {
        private readonly DapperContext _context;

        public CarePlanRepository(DapperContext context)
        {
            _context = context;
        }

        private const string BaseSelect = @"
            SELECT cp.*, p.FullName AS PatientName, c.FullName AS CaregiverName
            FROM CarePlans cp
            INNER JOIN Patients p ON cp.PatientId = p.PatientId
            INNER JOIN Caregivers c ON cp.CaregiverId = c.CaregiverId";

        public async Task<IEnumerable<CarePlan>> GetAllAsync()
        {
            var query = BaseSelect + " ORDER BY cp.CreatedAt DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CarePlan>(query);
        }

        public async Task<IEnumerable<CarePlan>> GetByPatientIdAsync(int patientId)
        {
            var query = BaseSelect + " WHERE cp.PatientId = @PatientId ORDER BY cp.CreatedAt DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CarePlan>(query, new { PatientId = patientId });
        }

        public async Task<CarePlan?> GetByIdAsync(int id)
        {
            var query = BaseSelect + " WHERE cp.CarePlanId = @CarePlanId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CarePlan>(query, new { CarePlanId = id });
        }

        public async Task<int> CreateAsync(CarePlan carePlan)
        {
            var query = @"INSERT INTO CarePlans (PatientId, CaregiverId, Title, Description, Medications, Goals, StartDate, EndDate, Status, CreatedAt)
                          OUTPUT INSERTED.CarePlanId
                          VALUES (@PatientId, @CaregiverId, @Title, @Description, @Medications, @Goals, @StartDate, @EndDate, @Status, @CreatedAt)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, carePlan);
        }

        public async Task<bool> UpdateAsync(CarePlan carePlan)
        {
            var query = @"UPDATE CarePlans SET Title=@Title, Description=@Description, Medications=@Medications,
                          Goals=@Goals, StartDate=@StartDate, EndDate=@EndDate, Status=@Status WHERE CarePlanId=@CarePlanId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, carePlan);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM CarePlans WHERE CarePlanId = @CarePlanId";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { CarePlanId = id });
            return rows > 0;
        }
    }
}
