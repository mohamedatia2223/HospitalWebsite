namespace Hospital.Interfaces.Repos
{
    public interface IReviewRepo
    {
        Task<List<ReviewForHospital>> GetAllReviews();
        Task AddReview(ReviewForHospital review);
        Task<ReviewForHospital> EditReviewByPatientId(Guid patientId,ReviewForHospital reviewDtoUpdate);
        public Task DeleteReviewByReviewId(Guid reviewId);
        Task<bool> ReviewExists(Guid reviewId);
        Task<int> GetCountOfAllReviews();
        Task<double> GetAverageofRating();
        Task<List<ReviewForHospital>> GetAllReviewsForPatient(Guid patientId);
    }
}
