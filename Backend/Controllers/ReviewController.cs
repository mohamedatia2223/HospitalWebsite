using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        public readonly IReviewService _reviewService;
        public readonly IPatientService _patientService;
        public ReviewController(IReviewService reviewService, IPatientService patientService)
        {
            _reviewService = reviewService;
            _patientService = patientService;
        }

        [HttpGet]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> GetAllReviews()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> GetAverageofRating()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var averageRating = await _reviewService.GetAverageofRating();
            return Ok(averageRating);
        }

        [HttpGet("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> GetCountOfAllReviews()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countOfAllReviews = await _reviewService.GetCountOfAllReviews();
            return Ok(countOfAllReviews);
        }

        [HttpGet("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> GetAllReviewsForPatient(Guid patientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (patientId == Guid.Empty)
            {
                return BadRequest("Patient ID cannot be empty");
            }
            if (!await _patientService.PatientExists(patientId))
            {
                return NotFound("Patient not found");
            }
            var reviews = await _reviewService.GetAllReviewsForPatient(patientId);
            return Ok(reviews);
        }

        [HttpPost("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status404NotFound))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> AddReviewByPatientId([FromForm] ReviewDTOGet reviewDTOGet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (reviewDTOGet.PatientId == Guid.Empty)
            {
                return BadRequest("Patient ID cannot be null or empty");
            }

            var patientExist = await _patientService.PatientExists(reviewDTOGet.PatientId);
            if (!patientExist)
            {
                return NotFound("Patient not found");
            }
            await _reviewService.AddReviewByPatientId(reviewDTOGet);
            return Ok("Review added successfully");
        }

        [HttpPut("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status404NotFound))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> EditReview(Guid ReviewId, [FromForm] ReviewDTOUpdate reviewDTOUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewExist = await _reviewService.ReviewExists(ReviewId);
            if(ReviewId==Guid.Empty)
            {
                return BadRequest("Review ID cannot be null or empty");
            }
            if (!reviewExist)
            {
                return NotFound("Review not found");
            }
            var patientReview = await _reviewService.EditReviewByPatientId(ReviewId, reviewDTOUpdate);

            return Ok(patientReview);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status404NotFound))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> DeleteReviewByReviewId(Guid ReviewId)
        {
            if (ReviewId == Guid.Empty)
            {
                return BadRequest("Review ID cannot be null or empty");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewExist = await _reviewService.ReviewExists(ReviewId);
            if (!reviewExist)
            {
                return NotFound("Review not found");
            }
            await _reviewService.DeleteReviewByReviewId(ReviewId);
            return Ok("Review deleted successfully");
        }

    }
}
