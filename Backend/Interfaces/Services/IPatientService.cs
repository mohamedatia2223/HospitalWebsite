using Hospital.Data.DTOs;

namespace Hospital.Interfaces.Services
{
	public interface IPatientService
	{
		public Task<List<PatientDTOGet>> GetAllPatients();
		public Task<PatientDTOGet> GetPatientById(Guid patientId);
		public Task<bool> PatientExists(Guid patientId);
		public Task AddPatient(PatientDTOUpdate patient);
		public Task UpdatePatientById(Guid patientId, PatientDTOUpdate patient);
		public Task DeletePatientById(Guid patientId);
		public Task<List<MedicalRecordDTOGet>> GetAllMedicalRecordsByPatientId(Guid patientId);
		public Task<List<AppointmentDTOGet>> GetAllAppointmentsByPatientId(Guid patientId);
		public Task<List<AppointmentDTOGet>> GetAllabcomingAppointmentsByPatientId(Guid patientId);
		public Task<List<PatientDTOGet>> FilterPatientsByName(string name);
		public Task AssignDoctorToPatient(Guid patientId,Guid doctorId);
		public Task RateDoctor(DoctorPatientDTO doctorPatientDTO);
		public Task<List<DoctorDTOGet>> GetAllDoctorsByPatientId(Guid patientId);
	}
}

