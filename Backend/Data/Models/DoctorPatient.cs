namespace Hospital.Data.Models
{
	public class DoctorPatient
	{
		[Key]
		public Guid PatientId { get; set; }
		public Patient? Patient { get; set; }
		[Key]
		public Guid DoctorId { get; set; }
		public Doctor? Doctor { get; set; }

		public int Rating { get; set; }
	}
}
