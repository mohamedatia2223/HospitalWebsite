namespace Hospital.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDTO>> GetAllAppointments();
        Task<AppointmentDTO> GetAppointmentById(Guid appointmentId);
        Task<bool> AppointmentExists(Guid appointmentId);
        Task AddAppointment(AppointmentDTO appointment);
        Task UpdateAppointmentById(Guid appointmentId, AppointmentDTO appointment);
        Task DeleteAppointmentById(Guid appointmentId);
        Task RescheduleAppointment(Guid appointmentId, DateTime newDateTime);
        Task<List<AppointmentDTO>> GetAppointmentsForToday();

    }
}
