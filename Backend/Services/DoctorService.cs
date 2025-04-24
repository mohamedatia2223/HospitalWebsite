using AutoMapper;
using Hospital.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services
{
	
	public class DoctorService : IDoctorService
	{
		private readonly IDoctorRepo _docRepo;
		private readonly IMapper _mapper;
        public DoctorService(IMapper mapper, IDoctorRepo docRepo)
		{
			_docRepo = docRepo;
            _mapper = mapper;

        }
		public async Task<bool> DoctorExists(Guid doctorId)
		{
			return await _docRepo.DoctorExists(doctorId);
		}
		public async Task AddDoctor(DoctorDTO doctor)
		{
			var doc = _mapper.Map<Doctor>(doctor);
			await _docRepo.AddDoctor(doc);
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
			
			return _mapper.Map<List<DoctorDTO>>(filtered);
		}

		

		public async Task<List<AppointmentDTO>> GetAllAppointmentsByDoctorId(Guid doctorId)
		{	
			

			var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
	

			return _mapper.Map<List<AppointmentDTO>>(doc.Appointments ?? []) ;
		}

		public async Task<List<DoctorDTO>> GetAllDoctors()
		{
			return _mapper.Map<List<DoctorDTO>>(await _docRepo.GetAllDoctors());
		}

		public async Task<List<PatientDTO>> GetAllPatientsByDoctorId(Guid doctorId)
		{
			

			var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
			var doctorPatients = doc.DoctorPatients;

			if (doc == null || doctorPatients == null)
				return [];

			var patients = doctorPatients.Select(d => d.Patient).ToList(); 

			return _mapper.Map<List<PatientDTO>>(patients);
		}

		public async Task<DoctorDTO> GetDoctorById(Guid doctorId)
		{
			
			return _mapper.Map<DoctorDTO>(await _docRepo.GetDoctorById(doctorId));
		}

		public async Task UpdateDoctorById(Guid doctorId, DoctorDTO doctor)
		{
            var doc = _mapper.Map<Doctor>(doctor);

            await _docRepo.UpdateDoctorById(doctorId, doc);
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
