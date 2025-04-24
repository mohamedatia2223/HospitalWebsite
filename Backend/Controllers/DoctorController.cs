namespace Hospital.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorController : ControllerBase
	{
		private readonly IDoctorService _docService;
		private readonly IPatientService _patientService;
		public DoctorController(IDoctorService docService, IPatientService patientService)
		{
			_docService = docService;
			_patientService = patientService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllDoctors()
		{
			var docs = await _docService.GetAllDoctors();
			return Ok(docs);
		}
		[HttpGet("{doctorId}")]
		public async Task<IActionResult> GetDoctorById(Guid doctorId)
		{
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			var doc = await _docService.GetDoctorById(doctorId);
			return Ok(doc);
		}
		[HttpPost]
		public async Task<IActionResult> AddDoctor([FromForm]DoctorDTO doctor)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
			await _docService.AddDoctor(doctor);
			return Created();
		}
		[HttpPut("{doctorId}")]
		public async Task<IActionResult> UpdateDoctorById(Guid doctorId, [FromForm]DoctorDTO doctor)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
			if (!await _docService.DoctorExists(doctorId)) {
				return NotFound("Doctor Not Found");
			}
			await _docService.UpdateDoctorById(doctorId,doctor);
			return NoContent();
		}
		[HttpDelete("{doctorId}")]
		public async Task<IActionResult> DeleteDoctorById(Guid doctorId)
		{
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			await _docService.DeleteDoctorById(doctorId);
			return NoContent();
		}
		[HttpGet("{doctorId}/patients")]
		public async Task<IActionResult> GetPatientsByDoctorId(Guid doctorId)
		{
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			var patients = await _docService.GetAllPatientsByDoctorId(doctorId);
			return Ok(patients);
		}
		[HttpGet("filter")]
		public async Task<IActionResult> FilterDoctors(string specialty,int minYears,string name)
		{
			var docs = await _docService.FilterDoctors(specialty,minYears,name);
			return Ok(docs);
		}
		[HttpGet("{doctorId}/appointments")]
		public async Task<IActionResult> GetAppointmentsByDoctorId(Guid doctorId)
		{
			
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			var appointments = await _docService.GetAllAppointmentsByDoctorId(doctorId);
			return Ok(appointments);
		}
		[HttpGet("{doctorId}/rating")]
		public async Task<IActionResult> GetAverageDoctorRating(Guid doctorId)
		{

			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			var rating = await _docService.GetAverageDoctorRating(doctorId);
			return Ok(rating);
		}
		[HttpGet("{doctorId}/rating/{patientId}")]
		public async Task<IActionResult> GetDoctorRatingByPatientId(Guid doctorId,Guid patientId)
		{
			if (!await _patientService.PatientExists(patientId))
			{
				return NotFound("Patient Not Found");
			}
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			// make it so it validates if it's assigned to a doctor or not 
			var rating = await _docService.GetDoctorRatingByPatientId(doctorId,patientId);
			return Ok(rating);
		}
	}
}
