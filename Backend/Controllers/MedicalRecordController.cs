using Hospital.Services;
using Hospital.Data.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddMedicalRecord([FromForm] MedicalRecordDTO dto)
        {
            await _service.AddMedicalRecord(dto);
            return Ok();
        }

        [HttpPut("UpdateMedicalRecordById/{id}")]
        public async Task<IActionResult> UpdateMedicalRecordById(Guid id, [FromForm] MedicalRecordDTO dto)
        {
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
            var records = await _service.SearchRecords(keyword);
            return Ok(records);
        }
    }
}
