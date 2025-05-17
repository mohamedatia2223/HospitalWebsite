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
		public async Task AddPatient(PatientDTOUpdate patient)
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

		public async Task<List<PatientDTOGet>> FilterPatientsByName(string name)
		{
			var patients = await _patientRepo.GetAllPatients();

			var regex = new Regex(name, RegexOptions.IgnoreCase); 
			var filtered = patients.Where(p => regex.IsMatch(p.PatientName)).ToList();

			return _mapper.Map<List<PatientDTOGet>>(filtered);
		}

		public async Task<List<AppointmentDTOGet>> GetAllabcomingAppointmentsByPatientId(Guid patientId)
		{
			var appointments = await GetAllAppointmentsByPatientId(patientId);
			
			return appointments.Where(a => a.AppointmentDate > DateTime.UtcNow).ToList();
		}

		public async Task<List<AppointmentDTOGet>> GetAllAppointmentsByPatientId(Guid patientId)
		{
			var patient =  await _patientRepo.GetPatientWithNavProp(patientId);

			return _mapper.Map<List<AppointmentDTOGet>>(patient.Appointments);
		}

		public async Task<List<DoctorDTOGet>> GetAllDoctorsByPatientId(Guid patientId)
		{
			var patient = await _patientRepo.GetPatientWithNavProp(patientId);
			if (patient.DoctorPatients == null)
			{
				patient.DoctorPatients = [];
			}
			var docs = patient.DoctorPatients.Select(dp => dp.Doctor).ToList();

			return _mapper.Map<List<DoctorDTOGet>>(docs);
		}

		public async Task<List<MedicalRecordDTOGet>> GetAllMedicalRecordsByPatientId(Guid patientId)
		{
			var patient = await _patientRepo.GetPatientWithNavProp(patientId);

			return _mapper.Map<List<MedicalRecordDTOGet>>(patient.MedicalRecords ?? []);
		}

		public async Task<List<PatientDTOGet>> GetAllPatients()
		{
			return _mapper.Map<List<PatientDTOGet>>(await _patientRepo.GetAllPatients());
		}

		public async Task<PatientDTOGet> GetPatientById(Guid patientId)
		{
			return _mapper.Map<PatientDTOGet>(await _patientRepo.GetPatientById(patientId));
		}

		public async Task<bool> PatientExists(Guid patientId)
		{
			return await _patientRepo.PatientExists(patientId);	
		}

		public async Task RateDoctor(DoctorPatientDTO doctorPatientDTO)
		{
			var doctorPatient = _mapper.Map<DoctorPatient>(doctorPatientDTO);
            await _patientRepo.RateDoctor(doctorPatient);
        }

		public async Task UpdatePatientById(Guid patientId, PatientDTOUpdate patient)
		{
			var pat = _mapper.Map<Patient>(patient);
			await _patientRepo.UpdatePatientById(patientId, pat);
		}
	}
}
