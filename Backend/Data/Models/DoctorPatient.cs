namespace Hospital.Data.Models
{
	public class DoctorPatient
	{
		[Key]
        public Guid DoctorPatientId { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
		public Patient? Patient { get; set; }
	
		public Guid DoctorId { get; set; }
		public Doctor? Doctor { get; set; }

		public double Rating { get; set; }
		public string? ReviewText { get; set; }

		public DateTime ReviewDate { get; set; } = DateTime.Now;
	}
}
