namespace HomeHealthcare.API.Models
{
    public class CarePlan
    {
        public int CarePlanId { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Medications { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Completed
        public DateTime CreatedAt { get; set; }

        public string? PatientName { get; set; }
        public string? CaregiverName { get; set; }
    }
}
