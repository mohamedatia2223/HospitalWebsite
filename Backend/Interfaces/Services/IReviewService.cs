namespace Hospital.Interfaces.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDTOGet>> GetAllReviews();
        Task AddReviewByPatientId(ReviewDTOUpdate review);
        Task<ReviewDTOGet> EditReviewByPatientId(Guid patientId,ReviewDTOUpdate reviewDtoUpdate);
        Task DeleteReviewByReviewId(Guid reviewId);
        Task<bool> ReviewExists(Guid reviewId);
        Task<double> GetAverageofRating();
        Task<int> GetCountOfAllReviews();
        Task<List<ReviewDTOGet>> GetAllReviewsForPatient(Guid patientId);
        Task<List<ReviewDTOGet>> FilterReviews(string keyword = "",string sortBy = "" , float rating = 0 ,int page = 1 , int pageSize = 10);
    }
}
