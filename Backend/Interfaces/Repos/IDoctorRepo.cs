namespace Hospital.Interfaces.Repos
{
	public interface IDoctorRepo
	{
		public Task<List<Doctor>> GetAllDoctors();
		public Task<Doctor> GetDoctorById(Guid doctorId);
		public Task<bool> DoctorExists(Guid doctorId);
		public Task AddDoctor(Doctor doctor);
		public Task UpdateDoctorById(Guid doctorId, Doctor doctor);
		public Task DeleteDoctorById(Guid doctorId);
		public Task<List<Doctor>> GetDoctorsWithNavProp();
		public Task<Doctor> GetDoctorWithNavProp(Guid doctorId);
        public Task<bool> IsAvailableAt(Guid doctorId, DateTime startTime , DateTime endTime , Guid? excludeAppId = null);
		public Task<List<DoctorPatient>> GetAllReviewsForDoctorById(Guid doctorId);
    }
}
