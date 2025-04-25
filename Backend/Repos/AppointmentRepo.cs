

namespace Hospital.Repos
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly HospitalContext _context;
        public AppointmentRepo(HospitalContext context)
        {
            _context = context;
        }
        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentById(Guid appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId);
        }

        public async Task AddAppointment(Appointment appointment)
        {
            await _context.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public Task<bool> AppointmentExists(Guid appointmentId)
        {
            return _context.Appointments.AnyAsync(a => a.AppointmentId == appointmentId);
        }
        public async Task UpdateAppointmentById(Guid appointmentId,Appointment appointment)
        {
            var app = await GetAppointmentById(appointmentId);
            if (app == null) throw new Exception("Appointment Not Found");
            app.AppointmentDate =appointment.AppointmentDate;
            app.ReasonForVisit = appointment.ReasonForVisit;
            await _context.SaveChangesAsync();

         }   
        public async Task DeleteAppointmentById(Guid appointmentId)
        {
            var app = await _context.Appointments.FindAsync(appointmentId);
            if (app == null) throw new Exception("Appointment Not Found");
            _context.Appointments.Remove(app);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsForToday()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now); 

            return await _context.Appointments
                .Where(a => DateOnly.FromDateTime(a.AppointmentDate) == today) 
                .ToListAsync();
        }
        public async Task SaveChanges()
        {
            
            await _context.SaveChangesAsync();
        }
    }
}
