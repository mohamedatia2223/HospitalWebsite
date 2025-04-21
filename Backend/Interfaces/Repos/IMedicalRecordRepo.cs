namespace Hospital.Interfaces.Repos
{
	public interface IMedicalRecordRepo
	{
		public Task<List<MedicalRecord>> GetAllMedicalRecords();
		public Task<MedicalRecord> GetMedicalRecordById(Guid medicalRecordId);
		public Task<bool> MedicalRecordExists(Guid medicalRecordId);
		public Task AddMedicalRecord(MedicalRecord medicalRecord);
		public Task UpdateMedicalRecordById(Guid medicalRecordId);
		public Task DeleteMedicalRecordById(Guid medicalRecordId);
	}
}
