namespace Hospital.Repos
{
	public class PatientRepo : IPatientRepo
	{	
		private readonly HospitalContext _context;
		public PatientRepo(HospitalContext context)
		{
			_context = context;
		}
		public async Task AddPatient(Patient patient)
		{
			await _context.Patients.AddAsync(patient);
			await _context.SaveChangesAsync();
		}

		public async Task DeletePatientById(Guid patientId)
		{
			var patient = await GetPatientById(patientId);
			_context.Remove(patient);

			await _context.SaveChangesAsync();
		}

		public async Task<List<Patient>> GetAllPatients()
		{
			return await _context.Patients.ToListAsync();
		}

		public async Task<Patient> GetPatientById(Guid patientId)
		{
			return await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId);

		}


		public async Task<List<Patient>> GetPatientsWithNavProp()
		{
			return await _context.Patients
				.Include(p => p.DoctorPatients)
				.ThenInclude(dp => dp.Doctor)
				.Include(p => p.Appointments)
				.Include(p => p.MedicalRecords).ToListAsync();
		}

		public async Task<Patient> GetPatientWithNavProp(Guid patientId)
		{
			return await _context.Patients
				.Include(p => p.DoctorPatients)
				.ThenInclude(dp => dp.Doctor)
				.Include(p => p.MedicalRecords)
				.Include(p => p.Appointments)
				.FirstOrDefaultAsync(p => p.PatientId == patientId);
				
		}

		public async Task<bool> PatientExists(Guid patientId)
		{
			return await _context.Patients.AnyAsync(p => p.PatientId == patientId);
		}

		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
		}

		public async Task UpdatePatientById(Guid patientId, Patient patient)
		{
			var oldPatient = await GetPatientById(patientId);

			oldPatient.PatientName = patient.PatientName;
			oldPatient.Email = patient.Email;
			oldPatient.Password = patient.Password;
			oldPatient.DateOfBirth = patient.DateOfBirth;
			oldPatient.ContactInfo = patient.ContactInfo;
			oldPatient.Address = patient.Address;
			oldPatient.InsuranceDetails = patient.InsuranceDetails;
			oldPatient.EmergencyContact = patient.EmergencyContact;

			await _context.SaveChangesAsync();

		}
	}
}
