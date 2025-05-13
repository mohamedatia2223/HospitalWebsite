namespace Hospital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpGet("{recordId}")]
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
        [Authorize(Roles = "Doctor,Admin")]        
        [HttpPost("AddMedicalRecord")]
        public async Task<IActionResult> AddMedicalRecord([FromBody] MedicalRecordDTOUpdate dto)
        {
            if (!ModelState.IsValid)
			{
				return BadRequest("Invalid Input");
			}
            await _service.AddMedicalRecord(dto);
            return Created();
        }

        [Authorize(Roles = "Doctor,Admin")]        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecordById(Guid id, [FromBody] MedicalRecordDTOUpdate dto)
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

        [Authorize(Roles = "Admin")]        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecordById(Guid id)
        {
            if (!await _service.MedicalRecordExists(id)){
                return NotFound("Record NOT FOUND 404") ;
            }
            await _service.DeleteMedicalRecordById(id);
            return NoContent();
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

        [HttpGet("SearchRecords/{keyword}")]
        public async Task<IActionResult> SearchRecords(string keyword)
        {
            if (keyword.Length < 1 ) {
                return BadRequest("Keyword input not valid");
            }
            var records = await _service.SearchRecords(keyword);
            return Ok(records);
        }
    }
}
