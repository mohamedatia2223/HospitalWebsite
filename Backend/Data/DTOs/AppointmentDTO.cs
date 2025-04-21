namespace Hospital.Data.DTOs
{
	public class AppointmentDTO
	{
		public Guid AppointmentId { get; set; }
		public DateTime AppointmentDate { get; set; }
		public string? ReasonForVisit { get; set; }

		public Guid? PatientId { get; set; }
		public Guid? DoctorId { get; set; }
	}
}
