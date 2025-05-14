namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
           var apps = await _appointmentService.GetAllAppointments();
            
            return Ok(apps);
        }
        [HttpGet("Today")]
        public async Task<IActionResult> GetAppointmentsForToday()
        {
            var apps = await _appointmentService.GetAppointmentsForToday();
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok(apps);
        }
        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddAppointment([FromForm]AppointmentDTOUpdate appointment)
        {
            await _appointmentService.AddAppointment(appointment);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Created();

        }
        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentById(Guid appointmentId,[FromForm] AppointmentDTOUpdate appointment)
        {  
            await _appointmentService.UpdateAppointmentById(appointmentId, appointment);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            if (! await _appointmentService.AppointmentExists(appointmentId)){
                return NotFound("Appointment NOT FOUND 404");
            }
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(Guid appointmentId)
        {
            await _appointmentService.DeleteAppointmentById(appointmentId);
            
            if (! await _appointmentService.AppointmentExists(appointmentId)){
                return NotFound("Appointment NOT FOUND 404");
            }
            return Ok();
        }
        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
        {
            var app = await _appointmentService.GetAppointmentById(appointmentId);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            if (! await _appointmentService.AppointmentExists(appointmentId)){
                return NotFound("Appointment NOT FOUND 404");
            }
            return Ok(app);
        }
        [HttpPut("{appointmentId}/Reschedule")]
        public async Task<IActionResult> RescheduleAppointment(Guid appointmentId, DateTime startTime,int Duration)
        {
            await _appointmentService.RescheduleAppointment(appointmentId, startTime,Duration);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            if (! await _appointmentService.AppointmentExists(appointmentId)){
                return NotFound("Appointment NOT FOUND 404");
            }
            if (startTime < DateTime.Now) {
                return BadRequest("You can't set the date in the past");
            }
            return NoContent();
        }
    }
}
