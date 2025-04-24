namespace Hospital.Interfaces.Services
{
	public interface IDoctorService
	{
		public Task<List<DoctorDTO>> GetAllDoctors();
		public Task<bool> DoctorExists(Guid doctorId);
		public Task<DoctorDTO> GetDoctorById(Guid doctorId);
		public Task AddDoctor(DoctorDTO doctor);
		public Task UpdateDoctorById(Guid doctorId, DoctorDTO doctor);
		public Task DeleteDoctorById(Guid doctorId);
		public Task<List<PatientDTO>> GetAllPatientsByDoctorId(Guid doctorId);
		public Task<List<DoctorDTO>> FilterDoctors(string specialty , int yearsOfExp,string name);
		public Task<List<AppointmentDTO>> GetAllAppointmentsByDoctorId(Guid doctorId);
		public Task<int?> GetDoctorRatingByPatientId(Guid doctorId, Guid patientId);
		public Task<float?> GetAverageDoctorRating(Guid doctorId);

    }
}
