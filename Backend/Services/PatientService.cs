using AutoMapper;

namespace Hospital.Services
{
	public class PatientService : IPatientService
	{
		private readonly IPatientRepo _patientRepo;
        private readonly IMapper _mapper;

        public PatientService(IMapper mapper, IPatientRepo patientRepo, HospitalContext context)
		{
			_patientRepo = patientRepo;
			_mapper = mapper;

        }
		public async Task AddPatient(PatientDTO patient)
		{
			var pat = _mapper.Map<Patient>(patient);
			await _patientRepo.AddPatient(pat);
		}

		// ليه الخره ده مش شغال انا حتجنن 😤😤😤😤😤😤😤😤
		public async Task AssignDoctorToPatient(Guid patientId, Guid doctorId)
		{
			var patient = await _patientRepo.GetPatientWithNavProp(patientId);


			if (patient.DoctorPatients == null)
				patient.DoctorPatients = new List<DoctorPatient>();


			if (!patient.DoctorPatients.Any(dp => dp.DoctorId == doctorId))
			{

				patient.DoctorPatients.Add(
					new DoctorPatient
					{
						DoctorId = doctorId,
						PatientId = patientId
					}
					);

				await _patientRepo.SaveChanges();
			}
		}

		public async Task DeletePatientById(Guid patientId)
		{
			await _patientRepo.DeletePatientById(patientId);
		}

		// NOTE: regex sucks ass and consider making the DB filter instead 

		public async Task<List<PatientDTO>> FilterPatientsByName(string name)
		{
			var patients = await _patientRepo.GetAllPatients();

			var regex = new Regex(name, RegexOptions.IgnoreCase); 
			var filtered = patients.Where(p => regex.IsMatch(p.PatientName)).ToList();

			return _mapper.Map<List<PatientDTO>>(filtered);
		}

		public async Task<List<AppointmentDTO>> GetAllabcomingAppointmentsByPatientId(Guid patientId)
		{
			var appointments = await GetAllAppointmentsByPatientId(patientId);
			
			return appointments.Where(a => a.AppointmentDate > DateTime.UtcNow).ToList();
		}

		public async Task<List<AppointmentDTO>> GetAllAppointmentsByPatientId(Guid patientId)
		{
			var patient =  await _patientRepo.GetPatientWithNavProp(patientId);

			return _mapper.Map<List<AppointmentDTO>>(patient.Appointments);
		}

		public async Task<List<DoctorDTO>> GetAllDoctorsByPatientId(Guid patientId)
		{
			var patient = await _patientRepo.GetPatientWithNavProp(patientId);
			if (patient.DoctorPatients == null)
			{
				patient.DoctorPatients = [];
			}
			var docs = patient.DoctorPatients.Select(dp => dp.Doctor).ToList();

			return _mapper.Map<List<DoctorDTO>>(docs);
		}

		public async Task<List<MedicalRecordDTO>> GetAllMedicalRecordsByPatientId(Guid patientId)
		{
			var patient = await _patientRepo.GetPatientWithNavProp(patientId);

			return _mapper.Map<List<MedicalRecordDTO>>(patient.MedicalRecords ?? []);
		}

		public async Task<List<PatientDTO>> GetAllPatients()
		{
			return _mapper.Map<List<PatientDTO>>(await _patientRepo.GetAllPatients());
		}

		public async Task<PatientDTO> GetPatientById(Guid patientId)
		{
			return _mapper.Map<PatientDTO>(await _patientRepo.GetPatientById(patientId));
		}

		public async Task<bool> PatientExists(Guid patientId)
		{
			return await _patientRepo.PatientExists(patientId);	
		}

		public async Task RateDoctor(Guid patientId, Guid doctorId, int rating)
		{
			var doctorPatient = (await _patientRepo.GetPatientWithNavProp(patientId)).DoctorPatients;
			if (doctorPatient == null)
			{
				doctorPatient = [];
			}
			var docpatient = doctorPatient.FirstOrDefault(dp => dp.DoctorId ==  doctorId);

			if (docpatient != null) {
				docpatient.Rating = rating;
			}
			await _patientRepo.SaveChanges();
		}

		public async Task UpdatePatientById(Guid patientId, PatientDTO patient)
		{
			var pat = _mapper.Map<Patient>(patient);
			await _patientRepo.UpdatePatientById(patientId, pat);
		}
	}
}
