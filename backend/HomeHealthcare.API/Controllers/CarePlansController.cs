using HomeHealthcare.API.Models;
using HomeHealthcare.API.Models.DTOs;
using HomeHealthcare.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHealthcare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CarePlansController : ControllerBase
    {
        private readonly ICarePlanRepository _carePlanRepository;

        public CarePlansController(ICarePlanRepository carePlanRepository)
        {
            _carePlanRepository = carePlanRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Caregiver")]
        public async Task<IActionResult> GetAll()
        {
            var carePlans = await _carePlanRepository.GetAllAsync();
            return Ok(carePlans);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var carePlans = await _carePlanRepository.GetByPatientIdAsync(patientId);
            return Ok(carePlans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var carePlan = await _carePlanRepository.GetByIdAsync(id);
            if (carePlan == null) return NotFound();
            return Ok(carePlan);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Caregiver")]
        public async Task<IActionResult> Create([FromBody] CreateCarePlanDto dto)
        {
            var carePlan = new CarePlan
            {
                PatientId = dto.PatientId,
                CaregiverId = dto.CaregiverId,
                Title = dto.Title,
                Description = dto.Description,
                Medications = dto.Medications,
                Goals = dto.Goals,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            var id = await _carePlanRepository.CreateAsync(carePlan);
            var created = await _carePlanRepository.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Caregiver")]
        public async Task<IActionResult> Update(int id, [FromBody] CarePlan carePlan)
        {
            carePlan.CarePlanId = id;
            var success = await _carePlanRepository.UpdateAsync(carePlan);
            if (!success) return NotFound();
            return Ok(new { message = "Care plan updated successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _carePlanRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Care plan deleted successfully." });
        }
    }
}
