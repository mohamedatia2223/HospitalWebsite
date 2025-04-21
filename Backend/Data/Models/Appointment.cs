
namespace Hospital.Data.Models
{
	public class Appointment
	{
		[Key]
		public Guid AppointmentId { get; set; } = Guid.NewGuid();
		public DateTime AppointmentDate { get; set; }
		public string? ReasonForVisit { get; set; }

		public Patient? Patient { get; set; }
		public Guid? PatientId { get; set; }
		public Doctor? Doctor { get; set; }
		public Guid? DoctorId { get; set; }

	}

}
