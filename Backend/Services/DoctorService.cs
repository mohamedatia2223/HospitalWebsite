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

		public async Task<List<DoctorDTO>> FilterDoctors(string specialty, int yearsOfExp)
		{
			var docs = await _docRepo.GetAllDoctors();
			var filtered = docs.Where(d => d.Specialty == specialty && d.YearsOfExperience >= yearsOfExp).ToList();
			
			return Mapper.MapDoctorDTOs(filtered);
		}

		

		public async Task<List<AppointmentDTO>> GetAllAppointmentsByDoctorId(Guid doctorId)
		{	
			

			var docs = await _docRepo.GetDoctorsWithNavProp();
			var doc = docs.FirstOrDefault(d => d.DoctorId == doctorId);
			

			return Mapper.MapAppointmentDTOs(doc.Appointments ?? []) ;
		}

		public async Task<List<DoctorDTO>> GetAllDoctors()
		{
			return Mapper.MapDoctorDTOs(await _docRepo.GetAllDoctors());
		}

		public async Task<List<PatientDTO>> GetAllPatientsByDoctorId(Guid doctorId)
		{
			

			var docs = await _docRepo.GetDoctorsWithNavProp();
			var doc = docs.FirstOrDefault(d => d.DoctorId == doctorId);


			return Mapper.MapPatientDTOs(doc.Patients ?? []);
		}

		public async Task<DoctorDTO> GetDoctorById(Guid doctorId)
		{
			
			return Mapper.MapDoctorDTO(await _docRepo.GetDoctorById(doctorId));
		}

		public async Task UpdateDoctorById(Guid doctorId, Doctor doctor)
		{
			
			await _docRepo.UpdateDoctorById(doctorId, doctor);
		}
	}
}
