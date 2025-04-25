namespace Hospital.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _repo;
        private readonly IPatientRepo _patientRepo;
        private readonly IDoctorRepo _doctorRepo;
        private readonly IMapper _mapper;

        public AppointmentService(
            IPatientRepo patientRepo,
            IMapper mapper,
            IDoctorRepo doctorRepo,
            IAppointmentRepo repo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
            _mapper = mapper;
            _patientRepo = patientRepo;
        }

        public async Task AddAppointment(AppointmentDTOUpdate dto)
        {
            var patient = await _patientRepo.GetPatientById(dto.PatientId);
            var doctor = await _doctorRepo.GetDoctorById(dto.DoctorId);

            if (patient == null || doctor == null)
                throw new Exception("Doctor or Patient not found");

            var startTime = dto.AppointmentDate ; 
            var endTime = startTime.AddHours(dto.Duration) ; 
            
            var available = await _doctorRepo.IsAvailableAt(dto.DoctorId,startTime,endTime);
            
            if (!available) { 
                throw new Exception("doctor busy");
            }
            var appointment = _mapper.Map<Appointment>(dto);

            await _repo.AddAppointment(appointment);
        }

        public Task<bool> AppointmentExists(Guid appointmentId)
        {
            return _repo.AppointmentExists(appointmentId);
        }

        public async Task DeleteAppointmentById(Guid appointmentId)
        {
            await _repo.DeleteAppointmentById(appointmentId);
        }

        public async Task<List<AppointmentDTOGet>> GetAllAppointments()
        {
            var appointments = await _repo.GetAllAppointments();
            return _mapper.Map<List<AppointmentDTOGet>>(appointments);
        }

        public async Task<AppointmentDTOGet> GetAppointmentById(Guid appointmentId)
        {
            var appointment = await _repo.GetAppointmentById(appointmentId);
            return _mapper.Map<AppointmentDTOGet>(appointment);
        }

        public async Task<List<AppointmentDTOGet>> GetAppointmentsForToday()
        {
            var todayAppointments = await _repo.GetAppointmentsForToday();
            return _mapper.Map<List<AppointmentDTOGet>>(todayAppointments);
        }

        public async Task RescheduleAppointment(Guid appointmentId, DateTime newDateTime , int Duration)
        {
            var appointment = await _repo.GetAppointmentById(appointmentId);
            
            if (appointment == null)
                throw new ArgumentException("Appointment not found");

            var doctorId = appointment.DoctorId;

            if (doctorId == null ) {
                throw new InvalidOperationException("Doctor ID null");
            }

            bool available = await _doctorRepo.IsAvailableAt(
                doctorId.Value, newDateTime , newDateTime.AddHours(Duration) ,appointmentId);
            if (!available)
                throw new InvalidOperationException("Doctor is not available at the new time");

            appointment.AppointmentDate = newDateTime;
            appointment.Duration = Duration;

            await _repo.SaveChanges();

        }

        public async Task UpdateAppointmentById(Guid appointmentId, AppointmentDTOUpdate dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            await _repo.UpdateAppointmentById(appointmentId, appointment);
        }
    }
}
