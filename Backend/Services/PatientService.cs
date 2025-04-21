using Microsoft.EntityFrameworkCore;

namespace Hospital.Services
{
	public class PatientService : IPatientService
	{
		private readonly IPatientRepo _patientRepo;
		private readonly HospitalContext _context;
		public PatientService(IPatientRepo patientRepo, HospitalContext context)
		{
			_patientRepo = patientRepo;
			_context = context;
		}
		public async Task AddPatient(Patient patient)
		{
			await _patientRepo.AddPatient(patient);
		}

		// ليه الخره ده مش شغال انا حتجنن 😤😤😤😤😤😤😤😤
		public async Task AssignDoctorToPatient(Guid patientId, Guid doctorId)
		{
			var patient = await _patientRepo.GetPatientById(patientId);

			patient.DoctorId = doctorId;

			await _patientRepo.SaveChanges();
		}

		public async Task DeletePatientById(Guid patientId)
		{
			await _patientRepo.DeletePatientById(patientId);
		}

		// NOTE: regex sucks ass and consider making the DB filter instead 

		public async Task<List<PatientDTO>> FilterPatientsByName(string name)
		{
			var patients = await _patientRepo.GetAllPatients();

			var regex = new Regex(name, RegexOptions.IgnoreCase); 
			var filtered = patients.Where(p => regex.IsMatch(p.PatientName)).ToList();

			return Mapper.MapPatientDTOs(filtered);
		}

		public async Task<List<AppointmentDTO>> GetAllabcomingAppointmentsByPatientId(Guid patientId)
		{
			var appointments = await GetAllAppointmentsByPatientId(patientId);
			
			return appointments.Where(a => a.AppointmentDate > DateTime.UtcNow).ToList();
		}

		public async Task<List<AppointmentDTO>> GetAllAppointmentsByPatientId(Guid patientId)
		{
			var patient =  await _patientRepo.GetPatientWithNavProp(patientId);

			return Mapper.MapAppointmentDTOs(patient.Appointments ?? []);
		}

		public async Task<List<MedicalRecordDTO>> GetAllMedicalRecordsByPatientId(Guid patientId)
		{
			var patient = await _patientRepo.GetPatientWithNavProp(patientId);

			return Mapper.MapMedicalRecordDTOs(patient.MedicalRecords);
		}

		public async Task<List<PatientDTO>> GetAllPatients()
		{
			return Mapper.MapPatientDTOs(await _patientRepo.GetAllPatients());
		}

		public async Task<PatientDTO> GetPatientById(Guid patientId)
		{
			return Mapper.MapPatientDTO(await _patientRepo.GetPatientById(patientId));
		}

		public async Task<bool> PatientExists(Guid patientId)
		{
			return await _patientRepo.PatientExists(patientId);	
		}

		public async Task UpdatePatientById(Guid patientId, Patient patient)
		{
			await _patientRepo.UpdatePatientById(patientId,patient);
		}
	}
}
