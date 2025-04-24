namespace Hospital.Interfaces.Services
{
    public interface IMedicalRecordService
    {
        public Task<List<MedicalRecordDTO>> GetAllMedicalRecords();
        public Task<MedicalRecordDTO> GetMedicalRecordById(Guid medicalRecordId);
        public Task<bool> MedicalRecordExists(Guid medicalRecordId);
        public Task AddMedicalRecord(MedicalRecordDTO medicalRecord);
        public Task UpdateMedicalRecordById(Guid medicalRecordId, MedicalRecordDTO mdeicalRecord);
        public Task DeleteMedicalRecordById(Guid medicalRecordId);
        public Task<List<MedicalRecordDTO>> GetRecordsByPatientId(Guid patientId);
        public Task<List<MedicalRecordDTO>> GetRecordsByDateRange(DateTime from, DateTime to);
        public Task<List<MedicalRecordDTO>> SearchRecords(string keyword);
    }
}
