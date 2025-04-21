namespace Hospital.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorController : ControllerBase
	{
		private readonly IDoctorService _docService;
		public DoctorController(IDoctorService docService)
		{
			_docService = docService;
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
			var doc = await _docService.GetDoctorById(doctorId);
			return Ok(doc);
		}
		[HttpPost]
		public async Task<IActionResult> AddDoctor(Doctor doctor)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
			await _docService.AddDoctor(doctor);
			return Created();
		}
		[HttpPut("{doctorId}")]
		public async Task<IActionResult> UpdateDoctorById(Guid doctorId,Doctor doctor)
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
		public async Task<IActionResult> FilterDoctors(string specialty,int minYears)
		{
			var docs = await _docService.FilterDoctors(specialty,minYears);
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
	}
}
