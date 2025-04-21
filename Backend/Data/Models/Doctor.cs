namespace Hospital.Data.Models
{
	[Index(nameof(DoctorName))]
	public class Doctor
	{
		[Key]
		public Guid DoctorId { get; set; } = Guid.NewGuid();
		[Required]
		[MaxLength(200)]
		public string? DoctorName { get; set; }
		[Required, MaxLength(200)]
		public string? Email { get; set; }
		[Required, MaxLength(200)]
		public string? Password { get; set; }
		[MaxLength(200)]
		public string? Specialty { get; set; }
		[MaxLength(20)]
		public string? ContactInfo { get; set; }
		public int YearsOfExperience { get; set; }
		public List<Appointment>? Appointments { get; set; }
		public List<Patient>? Patients { get; set; }

	}
}
