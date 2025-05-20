using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<DoctorDTOGet>> FilterDoctors(QueryObject query)
        {
            var filteredDocs = await _docRepo.GetDoctorsWithNavProp();
            var docs = filteredDocs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.DoctorName))
            {
                docs = docs.Where(s => s.DoctorName.Contains(query.DoctorName));
            }
            if (!string.IsNullOrWhiteSpace(query.Specialty))
            {
                docs = docs.Where(s => s.Specialty.Contains(query.Specialty));
            }
            if (query.YearsOfExperience.HasValue)
            {
                docs = docs.Where(s => s.YearsOfExperience >= query.YearsOfExperience);
            }

            var doctorsWithAvgRating = docs
                .Select(d => new
                {
                    Doctor = d,
                    AverageRating = d.DoctorPatients.Any() ?
                        d.DoctorPatients.Average(r => r.Rating) : 0
                });

            doctorsWithAvgRating = doctorsWithAvgRating.Where(da => da.AverageRating >= query.minRating);
            
            if (!string.IsNullOrWhiteSpace(query.SortBy) &&
                query.SortBy.Equals("Rating", StringComparison.OrdinalIgnoreCase))
            {

                doctorsWithAvgRating = query.IsDescending
                    ? doctorsWithAvgRating.OrderByDescending(s => s.AverageRating)
                    : doctorsWithAvgRating.OrderBy(s => s.AverageRating);

            }
            docs = doctorsWithAvgRating.Select(x => x.Doctor).AsQueryable();

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var newDocs = docs.Skip(skipNumber).Take(query.PageSize).ToList();

            return _mapper.Map<List<DoctorDTOGet>>(newDocs);
        }


        public async Task<List<AppointmentDTOGet>> GetAllAppointmentsByDoctorId(Guid doctorId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            return _mapper.Map<List<AppointmentDTOGet>>(doc.Appointments ?? []);
        }
        public async Task<List<AppointmentDTOGet>> GetAllUpcomingAppointmentsByDoctorId(Guid doctorId)
        {
            var apps = await GetAllAppointmentsByDoctorId(doctorId);
            
            return apps.Where( a => a.AppointmentDate > DateTime.Now).ToList();
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

        public async Task<double?> GetDoctorRatingByPatientId(Guid doctorId, Guid patientId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            var doctorPatient = doc.DoctorPatients.Find(dp => dp.PatientId == patientId);
            return doctorPatient?.Rating;
        }

        public async Task<double?> GetAverageDoctorRating(Guid doctorId)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId);
            var doctorPatients = doc.DoctorPatients;

            var rating = doctorPatients.Select(dp => dp.Rating).Where(r => r > 0).ToList();

            if (rating.Count == 0) return 0;

            return rating.Average(); 
        }
        public async Task<double> GetProfit(Guid doctorId, DateTime date)
        {
            var doc = await _docRepo.GetDoctorWithNavProp(doctorId) ;             
            var apps = doc.Appointments ; 
            if (apps == null) {
                apps = [];
            }

            var filteredApps = apps.Where(app => app.AppointmentDate <= date).ToList();

            double profit =  0 ; 
            foreach (var app in filteredApps)
            {
                profit += app.Duration * doc.HourlyPay ;
            }
            
            return profit ; 
        }

        public async Task<List<DoctorPatientDTO>> GetAllReviewsForDoctorById(Guid doctorId)
        {
            var result= await _docRepo.GetAllReviewsForDoctorById(doctorId);
            return _mapper.Map<List<DoctorPatientDTO>>(result);


        }
    }
}
