namespace Hospital.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDTOGet>> GetAllAppointments();
        Task<AppointmentDTOGet> GetAppointmentById(Guid appointmentId);
        Task<bool> AppointmentExists(Guid appointmentId);
        Task AddAppointment(AppointmentDTOUpdate appointment);
        Task UpdateAppointmentById(Guid appointmentId, AppointmentDTOUpdate appointment);
        Task DeleteAppointmentById(Guid appointmentId);
        Task RescheduleAppointment(Guid appointmentId, DateTime newDateTime , int Duration);
        Task<List<AppointmentDTOGet>> GetAppointmentsForToday();

    }
}
