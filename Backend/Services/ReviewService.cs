

namespace Hospital.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IMapper _mapper;
        public ReviewService(IMapper mapper, IReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<List<ReviewDTOGet>> GetAllReviews()
        {
            var reviews = await _reviewRepo.GetAllReviews();
            return _mapper.Map<List<ReviewDTOGet>>(reviews);

        }

        public async Task AddReviewByPatientId(ReviewDTOGet review)
        {
            var reviewToAdd = _mapper.Map<ReviewForHospital>(review);
            await _reviewRepo.AddReview(reviewToAdd);
        }

        public async Task<ReviewDTOGet> EditReviewByPatientId(Guid ReviewId,ReviewDTOUpdate reviewDtoUpdate)
        {
            var review = _mapper.Map<ReviewForHospital>(reviewDtoUpdate);
            var updatedReview = await _reviewRepo.EditReviewByPatientId(ReviewId, review);
            return _mapper.Map<ReviewDTOGet>(updatedReview);

        }

        public Task DeleteReviewByReviewId(Guid reviewId)
        {
            return _reviewRepo.DeleteReviewByReviewId(reviewId);
        }

        public async Task<double> GetAverageofRating()
        {
            return await _reviewRepo.GetAverageofRating();
        }

        public Task<int> GetCountOfAllReviews()
        {
            return _reviewRepo.GetCountOfAllReviews();
        }


        public Task<bool> ReviewExists(Guid reviewId)
        {
            return _reviewRepo.ReviewExists(reviewId);
        }

        public async Task<List<ReviewDTOGet>> GetAllReviewsForPatient(Guid patientId)
        {
            var reviews = await _reviewRepo.GetAllReviewsForPatient(patientId);
            return _mapper.Map<List<ReviewDTOGet>>(reviews);

        }
    }

}
