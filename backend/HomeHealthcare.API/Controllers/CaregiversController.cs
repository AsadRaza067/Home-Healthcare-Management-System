using HomeHealthcare.API.Models;
using HomeHealthcare.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHealthcare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CaregiversController : ControllerBase
    {
        private readonly ICaregiverRepository _caregiverRepository;

        public CaregiversController(ICaregiverRepository caregiverRepository)
        {
            _caregiverRepository = caregiverRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var caregivers = await _caregiverRepository.GetAllAsync();
            return Ok(caregivers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var caregiver = await _caregiverRepository.GetByIdAsync(id);
            if (caregiver == null) return NotFound();
            return Ok(caregiver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Caregiver caregiver)
        {
            caregiver.CaregiverId = id;
            var success = await _caregiverRepository.UpdateAsync(caregiver);
            if (!success) return NotFound();
            return Ok(new { message = "Caregiver updated successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _caregiverRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Caregiver deleted successfully." });
        }
    }
}
