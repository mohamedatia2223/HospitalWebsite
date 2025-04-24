using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok(apps);
        }
        [HttpGet("GetAppointmentsForToday")]
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
        public async Task<IActionResult> AddAppointment([FromForm]AppointmentDTO appointment)
        {
            await _appointmentService.AddAppointment(appointment);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok();

        }
        [HttpPut("UpdateAppointmentById")]
        public async Task<IActionResult> UpdateAppointmentById(Guid appointmentId,[FromForm] AppointmentDTO appointment)
        {  
            await _appointmentService.UpdateAppointmentById(appointmentId, appointment);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok();
        }
        [HttpDelete("")]
        public async Task<IActionResult> DeleteAppointment(Guid appointmentId)
        {
            await _appointmentService.DeleteAppointmentById(appointmentId);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok();
        }
        [HttpGet("GetAppointmentById")]
        public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
        {
            var app = await _appointmentService.GetAppointmentById(appointmentId);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok(app);
        }
        [HttpPut("RescheduleAppointment")]
        public async Task<IActionResult> RescheduleAppointment(Guid appointmentId, DateTime newDateTime)
        {
            await _appointmentService.RescheduleAppointment(appointmentId, newDateTime);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            return Ok();
        }
    }
}
