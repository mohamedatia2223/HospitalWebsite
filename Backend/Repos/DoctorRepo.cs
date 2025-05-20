
namespace Hospital.Repos
{
	public class DoctorRepo : IDoctorRepo
	{	
		private readonly HospitalContext _context ;
		public DoctorRepo(HospitalContext context)
		{
			_context = context;
		}
		public async Task<List<Doctor>> GetAllDoctors()
		{
			return await _context.Doctors.ToListAsync();
		}

		public async Task<Doctor> GetDoctorById(Guid doctorId)
		{
			var doc = await _context.Doctors.FindAsync(doctorId);

			return doc; 
		}
		public async Task<bool> DoctorExists(Guid doctorId)
		{
			return await _context.Doctors.AnyAsync(d => d.DoctorId == doctorId);
		}
		public async Task AddDoctor(Doctor doctor)
		{
			_context.Doctors.Add(doctor);
			await _context.SaveChangesAsync();
		}
		public async Task UpdateDoctorById(Guid doctorId, Doctor doctor)
		{
			Doctor? doc = await GetDoctorById(doctorId);

			if (doc == null) throw new Exception("Doctor Not Found");
			
			doc.DoctorName = doctor.DoctorName;
			doc.ContactInfo = doctor.ContactInfo;
			doc.Email = doctor.Email;
			doc.YearsOfExperience = doctor.YearsOfExperience;
			doc.Specialty = doctor.Specialty;
			doc.Password = doctor.Password;
			doc.HourlyPay = doctor.HourlyPay;

			await _context.SaveChangesAsync();

		}
		public async Task DeleteDoctorById(Guid doctorId)
		{
			Doctor? doc = await GetDoctorById(doctorId);

			_context.Doctors.Remove(doc);

			await _context.SaveChangesAsync();
		}

		public async Task<List<Doctor>> GetDoctorsWithNavProp()
		{
			return await _context.Doctors
				.Include(d => d.DoctorPatients)
				.ThenInclude(dp => dp.Patient)
				.Include(d => d.Appointments)
				.ToListAsync();
		}
		public async Task<Doctor> GetDoctorWithNavProp(Guid doctorId)
		{
			return await _context.Doctors
				.Include(d => d.DoctorPatients)
				.ThenInclude(dp => dp.Patient)
				.Include(d => d.Appointments)
				.FirstOrDefaultAsync(d => d.DoctorId == doctorId);
		}
        public async Task<bool> IsAvailableAt(Guid doctorId, DateTime startTime,DateTime endTime , Guid? excludeAppId = null )
        {	

			var doctor = await GetDoctorWithNavProp(doctorId);

            if (doctor.Appointments == null) {
                doctor.Appointments = [];
            }
			// here when reschulding an app , it also checks for the same app
			// if the doctor is busy , so we need to exclude this app 
            var apps = doctor.Appointments.Where(a => 
				(excludeAppId == null || a.AppointmentId != excludeAppId) &&
				a.AppointmentDate < endTime &&
                a.AppointmentDate.AddHours(a.Duration) > startTime);

            if (apps.Any()){ 
                return false ;
            }
			return true ; 
        }

        public async Task<List<DoctorPatient>> GetAllReviewsForDoctorById(Guid doctorId)
        {
            return await _context.DoctorPatients
                .Where(db => db.DoctorId == doctorId && string.IsNullOrEmpty(db.ReviewText) && db.Rating != 0)
                .ToListAsync();
        }
    }
}
