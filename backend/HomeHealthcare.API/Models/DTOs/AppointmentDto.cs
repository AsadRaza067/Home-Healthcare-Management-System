namespace HomeHealthcare.API.Models.DTOs
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
    }

    public class UpdateAppointmentStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string VisitNotes { get; set; } = string.Empty;
    }
}
