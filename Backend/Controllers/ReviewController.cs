namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

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
        public async Task<IActionResult> GetAllReviews()
        {
            List<ReviewDTOGet>? reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        public async Task<IActionResult> GetAverageofRating()
        {
            var averageRating = await _reviewService.GetAverageofRating();
            return Ok(averageRating);
        }

        [HttpGet("[action]")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        public async Task<IActionResult> GetCountOfAllReviews()
        {
            var countOfAllReviews = await _reviewService.GetCountOfAllReviews();
            return Ok(countOfAllReviews);
        }

        [HttpGet("[action]/{patientId}")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        public async Task<IActionResult> GetAllReviewsForPatient(Guid patientId)
        {
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
        [Authorize(Roles = "Patient,Admin")]

        public async Task<IActionResult> AddReviewByPatientId([FromBody] ReviewDTOUpdate reviewDTOUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (reviewDTOUpdate.PatientId == Guid.Empty)
            {
                return BadRequest("Patient ID cannot be null or empty");
            }

            var patientExist = await _patientService.PatientExists(reviewDTOUpdate.PatientId);
            if (!patientExist)
            {
                return NotFound("Patient not found");
            }
            await _reviewService.AddReviewByPatientId(reviewDTOUpdate);
            return Ok("Review added successfully");
        }

        [HttpPut("[action]/{ReviewId}")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status404NotFound))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> EditReview(Guid ReviewId, [FromBody] ReviewDTOUpdate reviewDTOUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewExist = await _reviewService.ReviewExists(ReviewId);
            if (ReviewId == Guid.Empty)
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

        [HttpDelete("[action]/{ReviewId}")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status404NotFound))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteReviewByReviewId(Guid ReviewId)
        {
            if (ReviewId == Guid.Empty)
            {
                return BadRequest("Review ID cannot be null or empty");
            }
            var reviewExist = await _reviewService.ReviewExists(ReviewId);
            if (!reviewExist)
            {
                return NotFound("Review not found");
            }
            await _reviewService.DeleteReviewByReviewId(ReviewId);
            return Ok("Review deleted successfully");
        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterReviews(string keyword = "", string sortBy = "", float rating = 0, int page = 1, int pageSize = 20)
        {
            var reviews = await _reviewService.FilterReviews(keyword, sortBy, rating, page, pageSize);
            return Ok(reviews);
        }

    }
}
