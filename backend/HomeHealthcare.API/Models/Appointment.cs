namespace HomeHealthcare.API.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
        public string VisitNotes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public string? PatientName { get; set; }
        public string? CaregiverName { get; set; }
    }
}
