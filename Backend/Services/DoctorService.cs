namespace Hospital.Services
{
	
	public class DoctorService : IDoctorService
	{
		private readonly IDoctorRepo _docRepo;
		public DoctorService(IDoctorRepo docRepo)
		{
			_docRepo = docRepo;
		}
		public async Task<bool> DoctorExists(Guid doctorId)
		{
			return await _docRepo.DoctorExists(doctorId);
		}
		public async Task AddDoctor(Doctor doctor)
		{
			await _docRepo.AddDoctor(doctor);
		}

		public async Task DeleteDoctorById(Guid doctorId)
		{
			
			await _docRepo.DeleteDoctorById(doctorId);
			
		}

		public async Task<List<DoctorDTO>> FilterDoctors(string specialty, int yearsOfExp ,string name)
		{
			var docs = await _docRepo.GetAllDoctors();
			var filtered = docs.Where(d => d.Specialty == specialty 
											&& d.YearsOfExperience >= yearsOfExp 
											&& d.DoctorName == name).ToList();
			
			return Mapper.MapDoctorDTOs(filtered);
		}

		

		public async Task<List<AppointmentDTO>> GetAllAppointmentsByDoctorId(Guid doctorId)
		{	
			

			var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
	

			return Mapper.MapAppointmentDTOs(doc.Appointments ?? []) ;
		}

		public async Task<List<DoctorDTO>> GetAllDoctors()
		{
			return Mapper.MapDoctorDTOs(await _docRepo.GetAllDoctors());
		}

		public async Task<List<PatientDTO>> GetAllPatientsByDoctorId(Guid doctorId)
		{
			

			var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
			var doctorPatients = doc.DoctorPatients;

			if (doc == null || doctorPatients == null)
				return [];

			var patients = doctorPatients.Select(d => d.Patient).ToList(); 

			return Mapper.MapPatientDTOs(patients);
		}

		public async Task<DoctorDTO> GetDoctorById(Guid doctorId)
		{
			
			return Mapper.MapDoctorDTO(await _docRepo.GetDoctorById(doctorId));
		}

		public async Task UpdateDoctorById(Guid doctorId, Doctor doctor)
		{
			
			await _docRepo.UpdateDoctorById(doctorId, doctor);
		}


		public async Task<int?> GetDoctorRatingByPatientId(Guid doctorId, Guid patientId)
		{
			var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
			var doctorPatients = doc.DoctorPatients;

			var doctorPatient = doctorPatients.FirstOrDefault(dp => dp.PatientId == patientId);
									

			return doctorPatient?.Rating;
		}

		public async Task<float?> GetAverageDoctorRating(Guid doctorId)
		{
			var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
			var doctorPatients = doc.DoctorPatients;

			var rating = doctorPatients.Select(dp => dp.Rating).Where(r => r > 0).ToList();

			if (rating.Count == 0) return 0;

			return rating.Sum()/rating.Count;
		}
	}
}
