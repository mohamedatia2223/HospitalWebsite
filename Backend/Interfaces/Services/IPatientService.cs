namespace Hospital.Interfaces.Services
{
	public interface IPatientService
	{
		public Task<List<PatientDTO>> GetAllPatients();
		public Task<PatientDTO> GetPatientById(Guid patientId);
		public Task<bool> PatientExists(Guid patientId);
		public Task AddPatient(Patient patient);
		public Task UpdatePatientById(Guid patientId,Patient patient);
		public Task DeletePatientById(Guid patientId);
		public Task<List<MedicalRecordDTO>> GetAllMedicalRecordsByPatientId(Guid patientId);
		public Task<List<AppointmentDTO>> GetAllAppointmentsByPatientId(Guid patientId);
		public Task<List<AppointmentDTO>> GetAllabcomingAppointmentsByPatientId(Guid patientId);
		public Task<List<PatientDTO>> FilterPatientsByName(string name);
		public Task AssignDoctorToPatient(Guid patientId,Guid doctorId);
		public Task RateDoctor(Guid patientId,Guid doctorId,int rating);
		public Task<List<DoctorDTO>> GetAllDoctorsByPatientId(Guid patientId);
	}
}

