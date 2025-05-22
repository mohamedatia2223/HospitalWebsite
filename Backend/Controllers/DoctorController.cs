namespace Hospital.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
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
        [Authorize(Roles = "Admin")]

		public async Task<IActionResult> AddDoctor([FromBody] DoctorDTOUpdate doctor)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
			await _docService.AddDoctor(doctor);
			return Created();
		}
		[Authorize(Roles="Admin")]
		[HttpPut("{doctorId}")]
		public async Task<IActionResult> UpdateDoctorById(Guid doctorId, [FromBody] DoctorDTOUpdate doctor)
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
		[Authorize(Roles="Admin")]
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
        public async Task<IActionResult> FilterDoctors([FromQuery] QueryObject query)
        {
            var docs = await _docService.FilterDoctors(query);

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
		[HttpGet("{doctorId}/upcomingAppointments")]
		public async Task<IActionResult> GetUpcomingAppointmentsByDoctorId(Guid doctorId)
		{
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			var appointments = await _docService.GetAllUpcomingAppointmentsByDoctorId(doctorId);

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
		[Authorize(Roles = "Doctor,Admin")]
		[HttpGet("{doctorId}/profit/{date}")]
		public async Task<IActionResult> GetDoctorSalary(Guid doctorId, DateTime? date = null)
		{	
			if (date == null) {
				date = DateTime.Now;
			}
			if (!await _docService.DoctorExists(doctorId))
			{
				return NotFound("Doctor Not Found");
			}
			if (date > DateTime.Now)
			{
				return BadRequest("Date cannot be in the future.");
			}

			var profit = await _docService.GetProfit(doctorId,date.Value);
			return Ok(profit);
		}
		//[HttpGet("{doctorId}/rating/{patientId}")]
		//public async Task<IActionResult> GetDoctorRatingByPatientId(Guid doctorId,Guid patientId)//// There is an Error
		//{
		//	if (!await _patientService.PatientExists(patientId))
		//	{
		//		return NotFound("Patient Not Found");
		//	}
		//	if (!await _docService.DoctorExists(doctorId))
		//	{
		//		return NotFound("Doctor Not Found");
		//	}
			
		//	var rating = await _docService.GetDoctorRatingByPatientId(doctorId,patientId);
		//	return Ok(rating);
		//}
		[HttpGet("[action]/{doctorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReviewsForDoctorById(Guid doctorId)
		{
            if (!await _docService.DoctorExists(doctorId))
            {
                return NotFound("Doctor Not Found");
            }
            var reviews = await _docService.GetAllReviewsForDoctorById(doctorId);
            return Ok(reviews);
        }
    }
}
