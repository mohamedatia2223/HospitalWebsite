
using AutoMapper;
using Hospital.Data.Models;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hospital.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _repo;
        private readonly IPatientRepo _patientRepo;
        private readonly IDoctorRepo _doctorRepo;
        private readonly IMapper _mapper;
        public AppointmentService(IPatientRepo petientRepo,IMapper mapper,IDoctorRepo doctorRepo, IAppointmentRepo repo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
            _mapper = mapper;
            _patientRepo = petientRepo;
        }

        public async Task AddAppointment(AppointmentDTO dto)
        {
            var patient = await _patientRepo.GetPatientById(dto.PatientId);
            var doctor = await _doctorRepo.GetDoctorById(dto.DoctorId);

            if (patient == null || doctor == null)
                throw new Exception("Doctor or Patient not found");

            var appointment = new Appointment
            {
                AppointmentDate = dto.AppointmentDate,
                ReasonForVisit = dto.ReasonForVisit,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
            };

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

        public async Task<List<AppointmentDTO>> GetAllAppointments()
        {
            return _mapper.Map<List<AppointmentDTO>>(await _repo.GetAllAppointments());
        }

        public async Task<AppointmentDTO> GetAppointmentById(Guid appointmentId)
        {
            return _mapper.Map<AppointmentDTO>(await _repo.GetAppointmentById(appointmentId));
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsForToday()
        {
            return _mapper.Map<List<AppointmentDTO>>(await _repo.GetAppointmentsForToday());
        }

        public async Task RescheduleAppointment(Guid appointmentId, DateTime newDateTime)
        {
            var appointment = await _repo.GetAppointmentById(appointmentId);
            var doctorId = appointment.DoctorId;

            if (appointment == null)
            {
                throw new ArgumentException("Appointment not found");
            }

            bool isAvailable = await _doctorRepo.IsAvailableAt(doctorId, newDateTime);

            if (!isAvailable)
            {
                throw new InvalidOperationException("Doctor is not available at the new time");
            }

            appointment.AppointmentDate = newDateTime;

            await _repo.UpdateAppointmentById(appointmentId, appointment);
        }

        public async Task UpdateAppointmentById(Guid appointmentId, AppointmentDTO appointment)
        {
            var app = _mapper.Map<Appointment>(appointment);
            await _repo.UpdateAppointmentById(appointmentId, app);
        }
    }
}
