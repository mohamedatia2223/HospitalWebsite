using AutoMapper;
using Hospital.Data.DTOs;

using Hospital.Interfaces.Services;

namespace Hospital.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepo _docRepo;
        private readonly IMapper _mapper;

        public DoctorService(IMapper mapper, IDoctorRepo docRepo)
        {
            _docRepo = docRepo;
            _mapper = mapper;
        }

        public async Task<bool> DoctorExists(Guid doctorId)
        {
            return await _docRepo.DoctorExists(doctorId);
        }

        public async Task AddDoctor(DoctorDTOUpdate doctor)
        {
            var doc = _mapper.Map<Doctor>(doctor);
            await _docRepo.AddDoctor(doc);
        }

        public async Task DeleteDoctorById(Guid doctorId)
        {
            await _docRepo.DeleteDoctorById(doctorId);
        }

        public async Task<List<DoctorDTOGet>> FilterDoctors(string specialty, int yearsOfExp, string name)
        {
            var docs = await _docRepo.GetAllDoctors();
            var filtered = docs
                .Where(d => d.Specialty == specialty
                            && d.YearsOfExperience >= yearsOfExp
                            && d.DoctorName == name)
                .ToList();

            return _mapper.Map<List<DoctorDTOGet>>(filtered);
        }

        public async Task<List<AppointmentDTOGet>> GetAllAppointmentsByDoctorId(Guid doctorId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            return _mapper.Map<List<AppointmentDTOGet>>(doc.Appointments ?? []);
        }

        public async Task<List<DoctorDTOGet>> GetAllDoctors()
        {
            var doctors = await _docRepo.GetAllDoctors();
            return _mapper.Map<List<DoctorDTOGet>>(doctors);
        }

        public async Task<List<PatientDTOGet>> GetAllPatientsByDoctorId(Guid doctorId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            var doctorPatients = doc?.DoctorPatients;

            if (doctorPatients == null)
                return [];

            var patients = doctorPatients.Select(dp => dp.Patient).ToList();
            return _mapper.Map<List<PatientDTOGet>>(patients);
        }

        public async Task<DoctorDTOGet> GetDoctorById(Guid doctorId)
        {
            var doctor = await _docRepo.GetDoctorById(doctorId);
            return _mapper.Map<DoctorDTOGet>(doctor);
        }

        public async Task UpdateDoctorById(Guid doctorId, DoctorDTOUpdate doctor)
        {
            var doc = _mapper.Map<Doctor>(doctor);
            await _docRepo.UpdateDoctorById(doctorId, doc);
        }

        public async Task<int?> GetDoctorRatingByPatientId(Guid doctorId, Guid patientId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            var doctorPatient = doc.DoctorPatients.FirstOrDefault(dp => dp.PatientId == patientId);
            return doctorPatient?.Rating;
        }

        public async Task<float?> GetAverageDoctorRating(Guid doctorId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            var doctorPatients = doc.DoctorPatients;

            var rating = doctorPatients.Select(dp => dp.Rating).Where(r => r > 0).ToList();

            if (rating.Count == 0) return 0;

            return rating.Sum() / rating.Count;
        }
    }
}
