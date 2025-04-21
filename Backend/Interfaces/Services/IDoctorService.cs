namespace Hospital.Interfaces.Services
{
	public interface IDoctorService
	{
		public Task<List<DoctorDTO>> GetAllDoctors();
		public Task<bool> DoctorExists(Guid doctorId);
		public Task<DoctorDTO> GetDoctorById(Guid doctorId);
		public Task AddDoctor(Doctor doctor);
		public Task UpdateDoctorById(Guid doctorId, Doctor doctor);
		public Task DeleteDoctorById(Guid doctorId);
		public Task<List<PatientDTO>> GetAllPatientsByDoctorId(Guid doctorId);
		public Task<List<DoctorDTO>> FilterDoctors(string specialty , int yearsOfExp);
		public Task<List<AppointmentDTO>> GetAllAppointmentsByDoctorId(Guid doctorId);
	}
}
