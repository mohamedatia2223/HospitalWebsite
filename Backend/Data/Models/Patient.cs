namespace Hospital.Data.Models
{

	[Index(nameof(PatientName))]
	public class Patient
	{
		[Key]
		public Guid PatientId { get; set; } = Guid.NewGuid(); 
		[Required]
		[MaxLength(200)]
		public required string PatientName { get; set; }
		[Required, MaxLength(200)]
		public required string Email { get; set; }
		[Required, MaxLength(200)]
		public required string Password { get; set; }
		[Required]
		public required DateTime DateOfBirth { get; set; }
		[MaxLength(200)]

		public string? ContactInfo { get; set; }
		[MaxLength(200)]
		public string? Address { get; set; }
		[MaxLength(200)]
		public string? InsuranceDetails { get; set; }
		[MaxLength(200)]
		public string? EmergencyContact { get; set; }
		public List<MedicalRecord>? MedicalRecords { get; set; }
		public List<Appointment>? Appointments { get; set; }
		public List<DoctorPatient>? DoctorPatients { get; set; }

        public ICollection<ReviewForHospital>? Reviews { get; set; }

    }
}
