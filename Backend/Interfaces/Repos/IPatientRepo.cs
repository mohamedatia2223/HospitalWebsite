namespace Hospital.Interfaces.Repos
{
	public interface IPatientRepo
	{	
		public Task<List<Patient>> GetAllPatients();
		public Task<Patient> GetPatientById(Guid patientId);
		public Task<bool> PatientExists(Guid patientId);
		public Task AddPatient(Patient patient);
		public Task UpdatePatientById(Guid patientId,Patient patient);
		public Task DeletePatientById(Guid patientId);
		public Task SaveChanges();
		public Task<List<Patient>> GetPatientsWithNavProp();
		public Task<Patient> GetPatientWithNavProp(Guid patientId);
	}
}
