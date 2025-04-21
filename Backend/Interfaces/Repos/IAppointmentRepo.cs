namespace Hospital.Interfaces.Repos
{
	public interface IAppointmentRepo
	{
		public Task<List<Appointment>> GetAllAppointments();
		public Task<Appointment> GetAppointmentById(Guid appointmentId);
		public Task<bool> AppointmentExists(Guid appointmentId);
		public Task AddAppointment(Appointment appointment);
		public Task UpdateAppointmentById(Guid appointmentId);
		public Task DeleteAppointmentById(Guid appointmentId);
	}
}
