namespace Hospital.Data.Models
{
	[Index(nameof(DoctorName))]
	public class Doctor
	{
		[Key]
		public Guid DoctorId { get; set; } = Guid.NewGuid();
		[Required]
		[MaxLength(200)]
		public required string DoctorName { get; set; }
		[Required, MaxLength(200)]
		public required string Email { get; set; }
		[Required, MaxLength(200)]
		public required string Password { get; set; }
		[Required]
		[MaxLength(200)]
		public required string Specialty { get; set; }
		public int HourlyPay { get; set; }
		[MaxLength(20)]
		public string? ContactInfo { get; set; }
		[Required]
		public int YearsOfExperience { get; set; }
		public List<Appointment>? Appointments { get; set; }
		public List<DoctorPatient>? DoctorPatients { get; set; }

	}
}
