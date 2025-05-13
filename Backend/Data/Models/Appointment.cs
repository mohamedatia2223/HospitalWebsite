
namespace Hospital.Data.Models
{
	public class Appointment
	{
		[Key]
		public Guid AppointmentId { get; set; } = Guid.NewGuid();
		[Required]
		public DateTime AppointmentDate { get; set; }
		public string? ReasonForVisit { get; set; }
		public int Duration { get; set; } = 1;  // duration is hours 
		public Patient? Patient { get; set; }
		public Guid? PatientId { get; set; }
		public Doctor? Doctor { get; set; }

		public Guid? DoctorId { get; set; }

	}

}
