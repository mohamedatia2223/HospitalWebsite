namespace Hospital.Data.Models
{

	[Index(nameof(PatientName))]
	public class Patient
	{
		[Key]
		public Guid PatientId { get; set; } = Guid.NewGuid(); 
		[Required]
		[MaxLength(200)]
		public string? PatientName { get; set; }
		[Required, MaxLength(200)]
		public string? Email { get; set; }
		[Required, MaxLength(200)]
		public string? Password { get; set; }
		public DateTime DateOfBirth { get; set; }
		[MaxLength(200)]

		public string? ContactInfo { get; set; }
		[MaxLength(200)]
		public string? Address { get; set; }
		[MaxLength(200)]
		public string? InsuranceDetails { get; set; }
		[MaxLength(200)]
		public string? EmergencyContact { get; set; }
		public Doctor? Doctor { get; set; }
		public Guid? DoctorId { get; set; }
		public List<MedicalRecord>? MedicalRecords { get; set; }
		public List<Appointment>? Appointments { get; set; }

	}
}
