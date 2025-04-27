namespace Hospital.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PatientController : ControllerBase
	{
		private readonly IPatientService _patientService;
		private readonly IDoctorService _docService;

		public PatientController(IPatientService patientService, IDoctorService doctorService)
		{
			_patientService = patientService;
			_docService = doctorService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllPatients()
		{
			var patients = await _patientService.GetAllPatients();
			return Ok(patients);
		}
		[HttpGet("{patientId}")]
		public async Task<IActionResult> GetPatientById(Guid patientId)
		{
			if (!await _patientService.PatientExists(patientId)) {
				return NotFound("Patient Not Found");
			}
			var patient = await _patientService.GetPatientById(patientId);
			return Ok(patient);
		}
		[HttpGet("{patientId}/records")]
		public async Task<IActionResult> GetAllMedicalRecordsByPatientId(Guid patientId)
		{
			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}
			var records = await _patientService.GetAllMedicalRecordsByPatientId(patientId);
			return Ok(records);
		}
		[HttpGet("{patientId}/appointments")]
		public async Task<IActionResult> GetAllAppointmentByPatientId(Guid patientId)
		{
			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}
			var appointments = await _patientService.GetAllAppointmentsByPatientId(patientId);
			return Ok(appointments);
		}
		[HttpGet("{patientId}/upcomingAppointments")]
		public async Task<IActionResult> GetAllUpcomingAppointmentByPatientId(Guid patientId)
		{
			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}
			var appointments = await _patientService.GetAllabcomingAppointmentsByPatientId(patientId);
			return Ok(appointments);
		}
		[HttpGet("filter")]
		public async Task<IActionResult> FilterPatients(string name)
		{	
			if (name.Length < 1) {
				return BadRequest("name input not Valid");
			}
			var patients = await _patientService.FilterPatientsByName(name);
			return Ok(patients);
		}
		[HttpPost("{patientId}/assignDoctor/{doctorId}")]
		public async Task<IActionResult> AssignDoctorToPatient(Guid patientId, Guid doctorId)
		{
			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}

			await _patientService.AssignDoctorToPatient(patientId,doctorId);

			return NoContent();
		}
		[HttpPost]
		public async Task<IActionResult> AddPatient([FromForm] PatientDTOUpdate patient)
		{

			if (!ModelState.IsValid) 
			{
				return BadRequest("Invalid Input");
			}

			await _patientService.AddPatient(patient);

			return Created();
		}
		[HttpPut]
		public async Task<IActionResult> UpdatePatient(Guid patientId,[FromForm] PatientDTOUpdate patient)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}

			await _patientService.UpdatePatientById(patientId,patient);

			return NoContent();
		}
		[HttpDelete]
		public async Task<IActionResult> DeletePatient(Guid patientId)
		{

			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}

			await _patientService.DeletePatientById(patientId);

			return NoContent();
		}
		[HttpGet("{patientId}/doctors")]
		public async Task<IActionResult> GetAllDoctorsByPatientId(Guid patientId)
		{

			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}

			var docs = await _patientService.GetAllDoctorsByPatientId(patientId);

			return Ok(docs);
		}
		[HttpPost("{patientId}/rateDoctor/{doctorId}")]
		public async Task<IActionResult> RateDoctor(Guid patientId,Guid doctorId, int rating)
		{

			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}

			await _patientService.RateDoctor(patientId,doctorId,rating);

			return NoContent();
		}

	}
}

