namespace Hospital.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepo _repo;
        private readonly IMapper _mapper;
        public MedicalRecordService(IMapper mapper, IMedicalRecordRepo repo)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task AddMedicalRecord(MedicalRecordDTOUpdate medicalRecord)
        {
            var MD = _mapper.Map<MedicalRecord>(medicalRecord);
            await _repo.AddMedicalRecord(MD);
        }

        public async Task DeleteMedicalRecordById(Guid medicalRecordId)
        {
            await  _repo.DeleteMedicalRecordById(medicalRecordId);
            
        }

        public async Task<List<MedicalRecordDTOGet>> GetAllMedicalRecords()
        {
            var MDs=  await _repo.GetAllMedicalRecords();
            if (!MDs.Any())
                throw new Exception("No medical records found.");

            return _mapper.Map<List<MedicalRecordDTOGet>>(MDs); 
        }

        public async Task<MedicalRecordDTOGet> GetMedicalRecordById(Guid medicalRecordId)
        {
            var MD = await _repo.GetMedicalRecordById(medicalRecordId);
            if (MD == null)
                throw new Exception("Medical record not found.");
            return _mapper.Map<MedicalRecordDTOGet>(MD);
        }

        public async Task<List<MedicalRecordDTOGet>> GetRecordsByDateRange(DateTime from, DateTime to)
        {
            var MDs = await _repo.GetRecordsByDateRange(from,to);
            if (!MDs.Any())
                throw new Exception("No medical records found.");

            return _mapper.Map<List<MedicalRecordDTOGet>>(MDs);
        }

        public async Task<List<MedicalRecordDTOGet>> GetRecordsByPatientId(Guid patientId)
        {
            var MDs = await _repo.GetRecordsByPatientId(patientId);
            if (!MDs.Any())
                throw new Exception("No medical records found.");

            return _mapper.Map<List<MedicalRecordDTOGet>>(MDs);
        }

        public async Task<bool> MedicalRecordExists(Guid medicalRecordId)
        {
            return await _repo.MedicalRecordExists(medicalRecordId);
        }

        public async Task<List<MedicalRecordDTOGet>> SearchRecords(string keyword)
        {
            var Mds = await _repo.SearchRecords(keyword);
            return _mapper.Map<List<MedicalRecordDTOGet>>(Mds);
        }

        public async Task UpdateMedicalRecordById(Guid medicalRecordId, MedicalRecordDTOUpdate mdeicalRecord)
        {
            var MD = _mapper.Map<MedicalRecord>(mdeicalRecord);
            await _repo.UpdateMedicalRecordById(medicalRecordId, MD);
        }
    }
}
