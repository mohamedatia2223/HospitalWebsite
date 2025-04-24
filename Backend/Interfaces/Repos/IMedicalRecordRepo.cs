namespace Hospital.Interfaces.Repos
{
	public interface IMedicalRecordRepo
	{
		public Task<List<MedicalRecord>> GetAllMedicalRecords();
		public Task<MedicalRecord> GetMedicalRecordById(Guid medicalRecordId);
		public Task<bool> MedicalRecordExists(Guid medicalRecordId);
		public Task AddMedicalRecord(MedicalRecord medicalRecord);
		public Task UpdateMedicalRecordById(Guid medicalRecordId,MedicalRecord mdeicalRecord);
		public Task DeleteMedicalRecordById(Guid medicalRecordId);
        public Task<List<MedicalRecord>> GetRecordsByDateRange(DateTime from, DateTime to);
        public Task<List<MedicalRecord>> GetRecordsByPatientId(Guid patientId);
		public Task<List<MedicalRecord>> SearchRecords(string keyword);


    }
}
