

namespace Hospital.Repos
{
    public class MedicalRecordRepo : IMedicalRecordRepo
    {
        private readonly HospitalContext _context;
        public MedicalRecordRepo(HospitalContext context)
        {
            _context = context;
        }
        public async Task AddMedicalRecord(MedicalRecord medicalRecord)
        {
            await _context.MedicalRecords.AddAsync(medicalRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicalRecordById(Guid medicalRecordId)
        {
            var MD = await _context.MedicalRecords.FindAsync(medicalRecordId);
            if (MD == null)
                throw new Exception("Medical Record Not Found");
            _context.MedicalRecords.Remove(MD);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MedicalRecord>> GetAllMedicalRecords()
        {
            return await _context.MedicalRecords.ToListAsync();
            
        }

        public async Task<MedicalRecord> GetMedicalRecordById(Guid medicalRecordId)
        {
            return await _context.MedicalRecords.SingleOrDefaultAsync(a=>a.MedicalRecordId == medicalRecordId);
            
        }

        public async Task<List<MedicalRecord>> GetRecordsByDateRange(DateTime from, DateTime to)
        {
            return await _context.MedicalRecords
                .Where(a => a.RecordDate >= from && a.RecordDate <= to)
                .ToListAsync();
        }

        public async Task<List<MedicalRecord>> GetRecordsByPatientId(Guid patientId)
        {
            return await _context.MedicalRecords
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<bool> MedicalRecordExists(Guid medicalRecordId)
        {
            return await _context.MedicalRecords.AnyAsync(a=>a.MedicalRecordId==medicalRecordId);
        }

        public async Task UpdateMedicalRecordById(Guid medicalRecordId, MedicalRecord mdeicalRecord )
        {
            var MD = await _context.MedicalRecords.SingleOrDefaultAsync(a => a.MedicalRecordId == medicalRecordId);

            if (MD == null)
                throw new Exception("Medical Record Not Found");

            MD.RecordDate = mdeicalRecord.RecordDate;
            MD.Treatment = mdeicalRecord.Treatment;
            MD.Notes = mdeicalRecord.Notes;
            MD.Diagnosis = mdeicalRecord.Diagnosis;
            await _context.SaveChangesAsync();
        }
        public async Task<List<MedicalRecord>> SearchRecords(string keyword)
        {
            return await _context.MedicalRecords
                .Where(m =>
                    m.Diagnosis.Contains(keyword) ||
                    m.Treatment.Contains(keyword) ||
                    m.Notes.Contains(keyword)
                )
                .ToListAsync();
        }

    }
}
