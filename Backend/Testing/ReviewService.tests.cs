using Hospital.Services;
using Hospital.Repos;
using AutoMapper;
using Moq;
using Xunit;
using Hospital.Data.DTOs;
using Hospital.Data.Models;
using Hospital.Interfaces.Repos;

namespace Hospital.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepo> _mockReviewRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ReviewService _reviewService;

        public ReviewServiceTests()
        {
            _mockReviewRepo = new Mock<IReviewRepo>();
            _mockMapper = new Mock<IMapper>();
            _reviewService = new ReviewService(_mockMapper.Object, _mockReviewRepo.Object);
        }

        [Fact]
        public async Task GetAllReviews_ReturnsMappedReviews()
        {
            // Arrange
            var reviews = new List<ReviewForHospital> { new ReviewForHospital(), new ReviewForHospital() };
            var expectedDtos = new List<ReviewDTOGet> { new ReviewDTOGet(), new ReviewDTOGet() };

            _mockReviewRepo.Setup(repo => repo.GetAllReviews()).ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<List<ReviewDTOGet>>(reviews)).Returns(expectedDtos);

            // Act
            var result = await _reviewService.GetAllReviews();

            // Assert
            Assert.Equal(expectedDtos, result);
            _mockReviewRepo.Verify(repo => repo.GetAllReviews(), Times.Once);
            _mockMapper.Verify(m => m.Map<List<ReviewDTOGet>>(reviews), Times.Once);
        }

        [Fact]
        public async Task AddReviewByPatientId_MapsAndAddsReview()
        {
            // Arrange
            var reviewDto = new ReviewDTOGet();
            var reviewToAdd = new ReviewForHospital();

            _mockMapper.Setup(m => m.Map<ReviewForHospital>(reviewDto)).Returns(reviewToAdd);
            _mockReviewRepo.Setup(repo => repo.AddReview(reviewToAdd)).Returns(Task.CompletedTask);

            // Act
            await _reviewService.AddReviewByPatientId(reviewDto);

            // Assert
            _mockMapper.Verify(m => m.Map<ReviewForHospital>(reviewDto), Times.Once);
            _mockReviewRepo.Verify(repo => repo.AddReview(reviewToAdd), Times.Once);
        }

        [Fact]
        public async Task EditReviewByPatientId_MapsAndUpdatesReview()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var reviewDtoUpdate = new ReviewDTOUpdate();
            var review = new ReviewForHospital();
            var updatedReview = new ReviewForHospital();
            var expectedDto = new ReviewDTOGet();

            _mockMapper.Setup(m => m.Map<ReviewForHospital>(reviewDtoUpdate)).Returns(review);
            _mockReviewRepo.Setup(repo => repo.EditReviewByPatientId(reviewId, review)).ReturnsAsync(updatedReview);
            _mockMapper.Setup(m => m.Map<ReviewDTOGet>(updatedReview)).Returns(expectedDto);

            // Act
            var result = await _reviewService.EditReviewByPatientId(reviewId, reviewDtoUpdate);

            // Assert
            Assert.Equal(expectedDto, result);
            _mockMapper.Verify(m => m.Map<ReviewForHospital>(reviewDtoUpdate), Times.Once);
            _mockReviewRepo.Verify(repo => repo.EditReviewByPatientId(reviewId, review), Times.Once);
            _mockMapper.Verify(m => m.Map<ReviewDTOGet>(updatedReview), Times.Once);
        }

        [Fact]
        public async Task DeleteReviewByReviewId_CallsRepository()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            _mockReviewRepo.Setup(repo => repo.DeleteReviewByReviewId(reviewId)).Returns(Task.CompletedTask);

            // Act
            await _reviewService.DeleteReviewByReviewId(reviewId);

            // Assert
            _mockReviewRepo.Verify(repo => repo.DeleteReviewByReviewId(reviewId), Times.Once);
        }

        [Fact]
        public async Task GetAverageofRating_ReturnsRepositoryValue()
        {
            // Arrange
            double expectedAverage = 4.5;
            _mockReviewRepo.Setup(repo => repo.GetAverageofRating()).ReturnsAsync(expectedAverage);

            // Act
            var result = await _reviewService.GetAverageofRating();

            // Assert
            Assert.Equal(expectedAverage, result);
            _mockReviewRepo.Verify(repo => repo.GetAverageofRating(), Times.Once);
        }

        [Fact]
        public async Task GetCountOfAllReviews_ReturnsRepositoryValue()
        {
            // Arrange
            int expectedCount = 10;
            _mockReviewRepo.Setup(repo => repo.GetCountOfAllReviews()).ReturnsAsync(expectedCount);

            // Act
            var result = await _reviewService.GetCountOfAllReviews();

            // Assert
            Assert.Equal(expectedCount, result);
            _mockReviewRepo.Verify(repo => repo.GetCountOfAllReviews(), Times.Once);
        }

        [Fact]
        public async Task ReviewExists_ReturnsRepositoryValue()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            bool expectedResult = true;
            _mockReviewRepo.Setup(repo => repo.ReviewExists(reviewId)).ReturnsAsync(expectedResult);

            // Act
            var result = await _reviewService.ReviewExists(reviewId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockReviewRepo.Verify(repo => repo.ReviewExists(reviewId), Times.Once);
        }

        [Fact]
        public async Task GetAllReviewsForPatient_ReturnsMappedReviews()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var reviews = new List<ReviewForHospital> { new ReviewForHospital(), new ReviewForHospital() };
            var expectedDtos = new List<ReviewDTOGet> { new ReviewDTOGet(), new ReviewDTOGet() };

            _mockReviewRepo.Setup(repo => repo.GetAllReviewsForPatient(patientId)).ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<List<ReviewDTOGet>>(reviews)).Returns(expectedDtos);

            // Act
            var result = await _reviewService.GetAllReviewsForPatient(patientId);

            // Assert
            Assert.Equal(expectedDtos, result);
            _mockReviewRepo.Verify(repo => repo.GetAllReviewsForPatient(patientId), Times.Once);
            _mockMapper.Verify(m => m.Map<List<ReviewDTOGet>>(reviews), Times.Once);
        }
    }
}