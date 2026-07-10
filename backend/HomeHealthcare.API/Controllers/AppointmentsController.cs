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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
            return Ok(appointments);
        }

        [HttpGet("caregiver/{caregiverId}")]
        public async Task<IActionResult> GetByCaregiver(int caregiverId)
        {
            var appointments = await _appointmentRepository.GetByCaregiverIdAsync(caregiverId);
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            // Prevent double-booking the same caregiver in the same slot
            var conflict = await _appointmentRepository.HasConflictAsync(dto.CaregiverId, dto.ScheduledDate, dto.TimeSlot);
            if (conflict)
                return Conflict(new { message = "This caregiver is already booked for the selected date and time slot." });

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                CaregiverId = dto.CaregiverId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = "Scheduled",
                VisitNotes = string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            var id = await _appointmentRepository.CreateAsync(appointment);
            var created = await _appointmentRepository.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Caregiver")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentStatusDto dto)
        {
            var success = await _appointmentRepository.UpdateStatusAsync(id, dto.Status, dto.VisitNotes);
            if (!success) return NotFound();
            return Ok(new { message = "Appointment status updated." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _appointmentRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Appointment deleted." });
        }
    }
}
