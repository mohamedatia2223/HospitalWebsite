using AutoMapper;
using Hospital.Data.DTOs;
using Hospital.Data.Models;
using Hospital.Interfaces.Repos;
using Hospital.Repos;
using Hospital.Services;
using Moq;
using Xunit;

namespace Hospital.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepo> _mockPatientRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _mockPatientRepo = new Mock<IPatientRepo>();
            _mockMapper = new Mock<IMapper>();
            _patientService = new PatientService(_mockMapper.Object, _mockPatientRepo.Object, null);
        }

        [Fact]
        public async Task AddPatient_ShouldMapAndCallRepo()
        {
            // Arrange
            var patientDto = new PatientDTOUpdate { Password = "testpassword123" };
            var patient = new Patient
            {
                PatientName = "John Doe", 
                Email = "john.doe@example.com", 
                Password = "aStrongPassword", 
                DateOfBirth = new DateTime(1985, 5, 15) };

            _mockMapper.Setup(m => m.Map<Patient>(patientDto)).Returns(patient);
            _mockPatientRepo.Setup(r => r.AddPatient(patient)).Returns(Task.CompletedTask);

            // Act
            await _patientService.AddPatient(patientDto);

            // Assert
            _mockMapper.Verify(m => m.Map<Patient>(patientDto), Times.Once);
            _mockPatientRepo.Verify(r => r.AddPatient(patient), Times.Once);
        }

        [Fact]
        public async Task AssignDoctorToPatient_WhenNotAlreadyAssigned_ShouldAddDoctorPatient()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var patient = new Patient
            {
                PatientName = "John Doe",
                Email = "john.doe@example.com",
                Password = "aStrongPassword",
                DateOfBirth = new DateTime(1985, 5, 15),
                DoctorPatients = new List<DoctorPatient>()
            };

            _mockPatientRepo.Setup(r => r.GetPatientWithNavProp(patientId)).ReturnsAsync(patient);
            _mockPatientRepo.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            await _patientService.AssignDoctorToPatient(patientId, doctorId);

            // Assert
            Assert.Single(patient.DoctorPatients);
            Assert.Equal(doctorId, patient.DoctorPatients[0].DoctorId);
            Assert.Equal(patientId, patient.DoctorPatients[0].PatientId);
            _mockPatientRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task AssignDoctorToPatient_WhenAlreadyAssigned_ShouldNotAddAgain()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var patient = new Patient
            {
                PatientName = "John Doe",
                Email = "john.doe@example.com",
                Password = "aStrongPassword",
                DateOfBirth = new DateTime(1985, 5, 15),
                DoctorPatients = new List<DoctorPatient>
                {
                    new DoctorPatient { DoctorId = doctorId, PatientId = patientId }
                }
            };

            _mockPatientRepo.Setup(r => r.GetPatientWithNavProp(patientId)).ReturnsAsync(patient);

            // Act
            await _patientService.AssignDoctorToPatient(patientId, doctorId);

            // Assert
            Assert.Single(patient.DoctorPatients);
            _mockPatientRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DeletePatientById_ShouldCallRepo()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockPatientRepo.Setup(r => r.DeletePatientById(patientId)).Returns(Task.CompletedTask);

            // Act
            await _patientService.DeletePatientById(patientId);

            // Assert
            _mockPatientRepo.Verify(r => r.DeletePatientById(patientId), Times.Once);
        }

    

        [Fact]
        public async Task GetAllPatients_ShouldReturnAllPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
               new Patient
                {
                    PatientName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Password = "AnotherSecurePassword",
                    DateOfBirth = new DateTime(1985, 5, 10)
                },
                 new Patient
                {
                    PatientName = "Peter Jones",
                    Email = "peter.jones@example.com",
                    Password = "YetAnotherSecurePassword",
                    DateOfBirth = new DateTime(1975, 11, 25)
                }
            };

            var patientDtos = new List<PatientDTOGet>
            {
                new PatientDTOGet(),
                new PatientDTOGet()
            };

            _mockPatientRepo.Setup(r => r.GetAllPatients()).ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<List<PatientDTOGet>>(patients)).Returns(patientDtos);

            // Act
            var result = await _patientService.GetAllPatients();

            // Assert
            Assert.Equal(2, result.Count);
            _mockPatientRepo.Verify(r => r.GetAllPatients(), Times.Once);
            _mockMapper.Verify(m => m.Map<List<PatientDTOGet>>(patients), Times.Once);
        }

        [Fact]
        public async Task PatientExists_ShouldReturnRepoResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            _mockPatientRepo.Setup(r => r.PatientExists(patientId)).ReturnsAsync(true);

            // Act
            var result = await _patientService.PatientExists(patientId);

            // Assert
            Assert.True(result);
            _mockPatientRepo.Verify(r => r.PatientExists(patientId), Times.Once);
        }

        [Fact]
        public async Task RateDoctor_ShouldMapAndCallRepo()
        {
            // Arrange
            var dto = new DoctorPatientDTO();
            var entity = new DoctorPatient();

            _mockMapper.Setup(m => m.Map<DoctorPatient>(dto)).Returns(entity);
            _mockPatientRepo.Setup(r => r.RateDoctor(entity)).Returns(Task.CompletedTask);

            // Act
            await _patientService.RateDoctor(dto);

            // Assert
            _mockMapper.Verify(m => m.Map<DoctorPatient>(dto), Times.Once);
            _mockPatientRepo.Verify(r => r.RateDoctor(entity), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientById_ShouldMapAndCallRepo()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patientDto = new PatientDTOUpdate { Password = "testpassword123" };
            var patient = new Patient
            {
                PatientName = "John Doe", 
                Email = "john.doe@example.com", 
                Password = "aStrongPassword", 
                DateOfBirth = new DateTime(1985, 5, 15)
            };

            _mockMapper.Setup(m => m.Map<Patient>(patientDto)).Returns(patient);
            _mockPatientRepo.Setup(r => r.UpdatePatientById(patientId, patient)).Returns(Task.CompletedTask);

            // Act
            await _patientService.UpdatePatientById(patientId, patientDto);

            // Assert
            _mockMapper.Verify(m => m.Map<Patient>(patientDto), Times.Once);
            _mockPatientRepo.Verify(r => r.UpdatePatientById(patientId, patient), Times.Once);
        }
    }
}