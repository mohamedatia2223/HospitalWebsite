namespace Hospital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _service;

        public MedicalRecordController(IMedicalRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            var records = await _service.GetAllMedicalRecords();
            return Ok(records);
        }

        [HttpGet("GetMedicalRecordById/{recordId}")]
        public async Task<IActionResult> GetMedicalRecordById(Guid recordId)
        {
            if (!await _service.MedicalRecordExists(recordId)){
                return NotFound("Record NOT FOUND 404") ;
            }
            try
            {
                var record = await _service.GetMedicalRecordById(recordId);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddMedicalRecord")]
        public async Task<IActionResult> AddMedicalRecord([FromForm] MedicalRecordDTOUpdate dto)
        {
            if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
            await _service.AddMedicalRecord(dto);
            return Created();
        }

        [HttpPut("UpdateMedicalRecordById/{id}")]
        public async Task<IActionResult> UpdateMedicalRecordById(Guid id, [FromForm] MedicalRecordDTOUpdate dto)
        {
            if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
            if (!await _service.MedicalRecordExists(id)){
                return NotFound("Record NOT FOUND 404") ;
            }
            try
            {
                await _service.UpdateMedicalRecordById(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteMedicalRecordById/{id}")]
        public async Task<IActionResult> DeleteMedicalRecordById(Guid id)
        {
            if (!await _service.MedicalRecordExists(id)){
                return NotFound("Record NOT FOUND 404") ;
            }
            await _service.DeleteMedicalRecordById(id);
            return NoContent();
        }

        [HttpGet("GetRecordsByPatientId/{patientId}")]
        public async Task<IActionResult> GetRecordsByPatientId(Guid patientId)
        {
            try
            {
                var records = await _service.GetRecordsByPatientId(patientId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {   
            if (from > DateTime.Now || to > DateTime.Now) {
                return BadRequest("Date can't be in the future");
            }
            try
            {
                var records = await _service.GetRecordsByDateRange(from, to);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("SearchRecords")]
        public async Task<IActionResult> SearchRecords([FromQuery] string keyword)
        {
            if (keyword.Length < 1 ) {
                return BadRequest("Keyword input not valid");
            }
            var records = await _service.SearchRecords(keyword);
            return Ok(records);
        }
    }
}
