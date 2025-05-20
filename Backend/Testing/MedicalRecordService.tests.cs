using AutoMapper;
using Hospital.Data.DTOs;
using Hospital.Data.Models;
using Hospital.Interfaces.Repos;
using Hospital.Services;
using Moq;
using Xunit;

namespace Hospital.Tests.Services
{
    public class MedicalRecordServiceTests
    {
        private readonly Mock<IMedicalRecordRepo> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MedicalRecordService _service;

        public MedicalRecordServiceTests()
        {
            _mockRepo = new Mock<IMedicalRecordRepo>();
            _mockMapper = new Mock<IMapper>();
            _service = new MedicalRecordService(_mockMapper.Object, _mockRepo.Object);
        }

        [Fact]
        public async Task AddMedicalRecord_ShouldCallAddMethod()
        {
            // Arrange
            var dto = new MedicalRecordDTOUpdate();
            var medicalRecord = new MedicalRecord();
            _mockMapper.Setup(m => m.Map<MedicalRecord>(dto)).Returns(medicalRecord);

            // Act
            await _service.AddMedicalRecord(dto);

            // Assert
            _mockRepo.Verify(r => r.AddMedicalRecord(medicalRecord), Times.Once);
            _mockMapper.Verify(m => m.Map<MedicalRecord>(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteMedicalRecordById_ShouldCallDeleteMethod()
        {
            // Arrange
            var medicalRecordId = Guid.NewGuid();
            _mockRepo.Setup(r => r.MedicalRecordExists(medicalRecordId)).ReturnsAsync(true);

            // Act
            await _service.DeleteMedicalRecordById(medicalRecordId);

            // Assert
            _mockRepo.Verify(r => r.DeleteMedicalRecordById(medicalRecordId), Times.Once);
        }

        

        [Fact]
        public async Task GetAllMedicalRecords_ShouldReturnRecords()
        {
            // Arrange
            var records = new List<MedicalRecord> { new MedicalRecord() };
            var recordDtos = new List<MedicalRecordDTOGet> { new MedicalRecordDTOGet() };

            _mockRepo.Setup(r => r.GetAllMedicalRecords()).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<List<MedicalRecordDTOGet>>(records)).Returns(recordDtos);

            // Act
            var result = await _service.GetAllMedicalRecords();

            // Assert
            Assert.Single(result);
            _mockRepo.Verify(r => r.GetAllMedicalRecords(), Times.Once);
            _mockMapper.Verify(m => m.Map<List<MedicalRecordDTOGet>>(records), Times.Once);
        }

        [Fact]
        public async Task GetAllMedicalRecords_WhenNoRecords_ShouldThrowException()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllMedicalRecords()).ReturnsAsync(new List<MedicalRecord>());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAllMedicalRecords());
        }

        [Fact]
        public async Task GetMedicalRecordById_ShouldReturnRecord()
        {
            // Arrange
            var recordId = Guid.NewGuid();
            var record = new MedicalRecord();
            var recordDto = new MedicalRecordDTOGet();

            _mockRepo.Setup(r => r.GetMedicalRecordById(recordId)).ReturnsAsync(record);
            _mockMapper.Setup(m => m.Map<MedicalRecordDTOGet>(record)).Returns(recordDto);

            // Act
            var result = await _service.GetMedicalRecordById(recordId);

            // Assert
            Assert.NotNull(result);
            _mockRepo.Verify(r => r.GetMedicalRecordById(recordId), Times.Once);
            _mockMapper.Verify(m => m.Map<MedicalRecordDTOGet>(record), Times.Once);
        }

        [Fact]
        public async Task GetMedicalRecordById_WhenRecordNotExists_ShouldThrowException()
        {
            // Arrange
            var recordId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetMedicalRecordById(recordId)).ReturnsAsync((MedicalRecord)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetMedicalRecordById(recordId));
        }

        [Fact]
        public async Task GetRecordsByDateRange_ShouldReturnRecords()
        {
            // Arrange
            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now;
            var records = new List<MedicalRecord> { new MedicalRecord() };
            var recordDtos = new List<MedicalRecordDTOGet> { new MedicalRecordDTOGet() };

            _mockRepo.Setup(r => r.GetRecordsByDateRange(from, to)).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<List<MedicalRecordDTOGet>>(records)).Returns(recordDtos);

            // Act
            var result = await _service.GetRecordsByDateRange(from, to);

            // Assert
            Assert.Single(result);
            _mockRepo.Verify(r => r.GetRecordsByDateRange(from, to), Times.Once);
            _mockMapper.Verify(m => m.Map<List<MedicalRecordDTOGet>>(records), Times.Once);
        }

        [Fact]
        public async Task GetRecordsByDateRange_WhenNoRecords_ShouldThrowException()
        {
            // Arrange
            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now;
            _mockRepo.Setup(r => r.GetRecordsByDateRange(from, to)).ReturnsAsync(new List<MedicalRecord>());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetRecordsByDateRange(from, to));
        }

        [Fact]
        public async Task GetRecordsByPatientId_ShouldReturnRecords()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var records = new List<MedicalRecord> { new MedicalRecord() };
            var recordDtos = new List<MedicalRecordDTOGet> { new MedicalRecordDTOGet() };

            _mockRepo.Setup(r => r.GetRecordsByPatientId(patientId)).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<List<MedicalRecordDTOGet>>(records)).Returns(recordDtos);

            // Act
            var result = await _service.GetRecordsByPatientId(patientId);

            // Assert
            Assert.Single(result);
            _mockRepo.Verify(r => r.GetRecordsByPatientId(patientId), Times.Once);
            _mockMapper.Verify(m => m.Map<List<MedicalRecordDTOGet>>(records), Times.Once);
        }

        [Fact]
        public async Task GetRecordsByPatientId_WhenNoRecords_ShouldThrowException()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetRecordsByPatientId(patientId)).ReturnsAsync(new List<MedicalRecord>());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetRecordsByPatientId(patientId));
        }

        [Fact]
        public async Task MedicalRecordExists_ShouldReturnResultFromRepo()
        {
            // Arrange
            var recordId = Guid.NewGuid();
            _mockRepo.Setup(r => r.MedicalRecordExists(recordId)).ReturnsAsync(true);

            // Act
            var result = await _service.MedicalRecordExists(recordId);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.MedicalRecordExists(recordId), Times.Once);
        }

        [Fact]
        public async Task SearchRecords_ShouldReturnMappedResults()
        {
            // Arrange
            var keyword = "test";
            var records = new List<MedicalRecord> { new MedicalRecord() };
            var recordDtos = new List<MedicalRecordDTOGet> { new MedicalRecordDTOGet() };

            _mockRepo.Setup(r => r.SearchRecords(keyword)).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<List<MedicalRecordDTOGet>>(records)).Returns(recordDtos);

            // Act
            var result = await _service.SearchRecords(keyword);

            // Assert
            Assert.Single(result);
            _mockRepo.Verify(r => r.SearchRecords(keyword), Times.Once);
            _mockMapper.Verify(m => m.Map<List<MedicalRecordDTOGet>>(records), Times.Once);
        }

        [Fact]
        public async Task UpdateMedicalRecordById_ShouldCallUpdateMethod()
        {
            // Arrange
            var recordId = Guid.NewGuid();
            var dto = new MedicalRecordDTOUpdate();
            var medicalRecord = new MedicalRecord();

            _mockRepo.Setup(r => r.MedicalRecordExists(recordId)).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<MedicalRecord>(dto)).Returns(medicalRecord);

            // Act
            await _service.UpdateMedicalRecordById(recordId, dto);

            // Assert
            _mockRepo.Verify(r => r.UpdateMedicalRecordById(recordId, medicalRecord), Times.Once);
            _mockMapper.Verify(m => m.Map<MedicalRecord>(dto), Times.Once);
        }

        
    }
}