namespace HomeHealthcare.API.Models.DTOs
{
    public class CreateCarePlanDto
    {
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Medications { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
