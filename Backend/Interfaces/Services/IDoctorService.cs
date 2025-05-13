using Finance_Project.Helper;

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
		public Task<List<DoctorDTOGet>> FilterDoctors(QueryObject query);
		public Task<List<AppointmentDTOGet>> GetAllAppointmentsByDoctorId(Guid doctorId);
		public Task<int?> GetDoctorRatingByPatientId(Guid doctorId, Guid patientId);
		public Task<float?> GetAverageDoctorRating(Guid doctorId);
		Task<List<AppointmentDTOGet>> GetAllUpcomingAppointmentsByDoctorId(Guid doctorId);
		Task<double> GetProfit(Guid doctorId, DateTime date);

    }
}
