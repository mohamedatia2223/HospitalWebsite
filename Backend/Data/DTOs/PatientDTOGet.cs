namespace Hospital.Data.DTOs
{
	public class PatientDTOGet
    {
        public Guid PatientId { get; set; } = Guid.NewGuid();
		public string? PatientName { get; set; }
		public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
		public string? ContactInfo { get; set; }
		public string? Address { get; set; }
		public string? InsuranceDetails { get; set; }
		public string? EmergencyContact { get; set; }
	}
}
