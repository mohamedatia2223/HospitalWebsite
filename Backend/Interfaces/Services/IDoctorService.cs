namespace Hospital.Interfaces.Services
{
	public interface IDoctorService
	{
		public Task<List<DoctorDTOGet>> GetAllDoctors();
		public Task<bool> DoctorExists(Guid doctorId);
		public Task<DoctorDTOGet> GetDoctorById(Guid doctorId);
		public Task AddDoctor(DoctorDTOUpdate doctor);
		public Task UpdateDoctorById(Guid doctorId, DoctorDTOUpdate doctor);
		public Task DeleteDoctorById(Guid doctorId);
		public Task<List<PatientDTOGet>> GetAllPatientsByDoctorId(Guid doctorId);
		public Task<List<DoctorDTOGet>> FilterDoctors(string specialty , int yearsOfExp,string name);
		public Task<List<AppointmentDTOGet>> GetAllAppointmentsByDoctorId(Guid doctorId);
		public Task<double?> GetDoctorRatingByPatientId(Guid doctorId, Guid patientId);
		public Task<double?> GetAverageDoctorRating(Guid doctorId);
		Task<List<AppointmentDTOGet>> GetAllUpcomingAppointmentsByDoctorId(Guid doctorId);
		Task<double> GetProfit(Guid doctorId, DateTime date);
		public Task<List<DoctorPatientDTO>> GetAllReviewsForDoctorById(Guid doctorId);

    }
}
