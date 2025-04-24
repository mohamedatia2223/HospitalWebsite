namespace Hospital.Interfaces.Services
{
    public interface IMedicalRecordService
    {
        public Task<List<MedicalRecordDTOGet>> GetAllMedicalRecords();
        public Task<MedicalRecordDTOGet> GetMedicalRecordById(Guid medicalRecordId);
        public Task<bool> MedicalRecordExists(Guid medicalRecordId);
        public Task AddMedicalRecord(MedicalRecordDTOUpdate medicalRecord);
        public Task UpdateMedicalRecordById(Guid medicalRecordId, MedicalRecordDTOUpdate mdeicalRecord);
        public Task DeleteMedicalRecordById(Guid medicalRecordId);
        public Task<List<MedicalRecordDTOGet>> GetRecordsByPatientId(Guid patientId);
        public Task<List<MedicalRecordDTOGet>> GetRecordsByDateRange(DateTime from, DateTime to);
        public Task<List<MedicalRecordDTOGet>> SearchRecords(string keyword);
    }
}
