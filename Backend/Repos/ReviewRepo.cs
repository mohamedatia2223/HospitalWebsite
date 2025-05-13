

namespace Hospital.Repos
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly HospitalContext _context;
        public ReviewRepo(HospitalContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewForHospital>> GetAllReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task AddReview(ReviewForHospital review)
        {
          await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewByReviewId(Guid reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null)
            {
                throw new Exception("Review not found.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }


        public async Task<ReviewForHospital> EditReviewByPatientId(Guid ReviewId, ReviewForHospital reviewDtoUpdate)
        {
            var review =await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == ReviewId);
            review!.ReviewDate = reviewDtoUpdate.ReviewDate;
            review.Rating = reviewDtoUpdate.Rating;
            review.ReviewText = reviewDtoUpdate.ReviewText;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }


        public Task<double> GetAverageofRating()
        {
           return _context.Reviews.AverageAsync(r => r.Rating);
        }

        public Task<int> GetCountOfAllReviews()
        {
            return _context.Reviews.CountAsync();
        }



        public Task<bool> ReviewExists(Guid reviewId)
        {
            return _context.Reviews.AnyAsync(r => r.ReviewId == reviewId);
        }

        public Task<List<ReviewForHospital>> GetAllReviewsForPatient(Guid patientId)
        {
           return _context.Reviews.Where(r => r.PatientId == patientId).ToListAsync();
        }
    }
}
