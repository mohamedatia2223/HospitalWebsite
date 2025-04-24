namespace Hospital.Data.DTOs
{
	public class AppointmentDTOGet
	{
        public Guid AppointmentId { get; set; } = Guid.NewGuid();

        public DateTime AppointmentDate { get; set; }
		public string? ReasonForVisit { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }

    }
}
