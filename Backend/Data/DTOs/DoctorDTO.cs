namespace Hospital.Data.DTOs
{
	public class DoctorDTO
	{
		
		public Guid DoctorId { get; set; }
		public string? DoctorName { get; set; }
		public string? Email { get; set; }
		public string? Specialty { get; set; }
		public string? ContactInfo { get; set; }
		public int YearsOfExperience { get; set; }
	}
}
