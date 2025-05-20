using Hospital.Services;
using AutoMapper;
using Moq;
using Xunit;
using Hospital.Data.DTOs;
using Hospital.Data.Models;
using Hospital.Interfaces.Repos;

namespace Hospital.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepo> _mockAppointmentRepo;
        private readonly Mock<IPatientRepo> _mockPatientRepo;
        private readonly Mock<IDoctorRepo> _mockDoctorRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _mockAppointmentRepo = new Mock<IAppointmentRepo>();
            _mockPatientRepo = new Mock<IPatientRepo>();
            _mockDoctorRepo = new Mock<IDoctorRepo>();
            _mockMapper = new Mock<IMapper>();

            _service = new AppointmentService(
                _mockPatientRepo.Object,
                _mockMapper.Object,
                _mockDoctorRepo.Object,
                _mockAppointmentRepo.Object);
        }

        [Fact]
        public async Task AddAppointment_ValidData_ShouldAddAppointment()
        {
            // Arrange
            var dto = new AppointmentDTOUpdate
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(1),
                Duration = 1
            };

            var patient = new Patient
            {
                PatientName = "Jane Smith",
                Email = "jane.smith@example.com",
                Password = "AnotherSecurePassword",
                DateOfBirth = new DateTime(1985, 5, 10)
            };
            var doctor = new Doctor
            {
                DoctorName = "Dr. John Doe", 
                Email = "john.doe@example.com", 
                Password = "securepassword", 
                Specialty = "General Medicine" 
            };
            var appointment = new Appointment();

            _mockPatientRepo.Setup(r => r.GetPatientById(dto.PatientId))
                .ReturnsAsync(patient);
            _mockDoctorRepo.Setup(r => r.GetDoctorById(dto.DoctorId))
                .ReturnsAsync(doctor);
            _mockDoctorRepo.Setup(r => r.IsAvailableAt(dto.DoctorId,It.IsAny<DateTime>(),It.IsAny<DateTime>(),It.IsAny<Guid?>() )) .ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<Appointment>(dto))
                .Returns(appointment);

            // Act
            await _service.AddAppointment(dto);

            // Assert
            _mockAppointmentRepo.Verify(r => r.AddAppointment(appointment), Times.Once);
        }

        [Fact]
        public async Task AddAppointment_PatientOrDoctorNotFound_ShouldThrowException()
        {
            // Arrange
            var dto = new AppointmentDTOUpdate
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid()
            };

            _mockPatientRepo.Setup(r => r.GetPatientById(dto.PatientId))
                .ReturnsAsync((Patient)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.AddAppointment(dto));
        }

       

        [Fact]
        public async Task RescheduleAppointment_ValidData_ShouldUpdateAppointment()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var newDateTime = DateTime.Now.AddDays(2);
            var duration = 2;

            var existingAppointment = new Appointment
            {
                Id = appointmentId,
                DoctorId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(1),
                Duration = 1
            };

            _mockAppointmentRepo.Setup(r => r.GetAppointmentById(appointmentId))
                .ReturnsAsync(existingAppointment);
            _mockDoctorRepo.Setup(r => r.IsAvailableAt(
                existingAppointment.DoctorId.Value,
                newDateTime,
                newDateTime.AddHours(duration),
                appointmentId))
                .ReturnsAsync(true);

            // Act
            await _service.RescheduleAppointment(appointmentId, newDateTime, duration);

            // Assert
            Assert.Equal(newDateTime, existingAppointment.AppointmentDate);
            Assert.Equal(duration, existingAppointment.Duration);
            _mockAppointmentRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task RescheduleAppointment_AppointmentNotFound_ShouldThrowException()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();

            _mockAppointmentRepo.Setup(r => r.GetAppointmentById(appointmentId))
                .ReturnsAsync((Appointment)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.RescheduleAppointment(appointmentId, DateTime.Now, 1));
        }

        [Fact]
        public async Task RescheduleAppointment_DoctorNotAvailable_ShouldThrowException()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var newDateTime = DateTime.Now.AddDays(2);

            var existingAppointment = new Appointment
            {
                Id = appointmentId,
                DoctorId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(1),
                Duration = 1
            };

            _mockAppointmentRepo.Setup(r => r.GetAppointmentById(appointmentId))
                .ReturnsAsync(existingAppointment);
            _mockDoctorRepo.Setup(r => r.IsAvailableAt(
                existingAppointment.DoctorId.Value,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                appointmentId))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.RescheduleAppointment(appointmentId, newDateTime, 1));
        }

        [Fact]
        public async Task GetAppointmentById_ValidId_ShouldReturnAppointment()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment { Id = appointmentId };
            var expectedDto = new AppointmentDTOGet();

            _mockAppointmentRepo.Setup(r => r.GetAppointmentById(appointmentId))
                .ReturnsAsync(appointment);
            _mockMapper.Setup(m => m.Map<AppointmentDTOGet>(appointment))
                .Returns(expectedDto);

            // Act
            var result = await _service.GetAppointmentById(appointmentId);

            // Assert
            Assert.Equal(expectedDto, result);
        }

        [Fact]
        public async Task GetAllAppointments_ShouldReturnAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment> { new Appointment() };
            var expectedDtos = new List<AppointmentDTOGet> { new AppointmentDTOGet() };

            _mockAppointmentRepo.Setup(r => r.GetAllAppointments())
                .ReturnsAsync(appointments);
            _mockMapper.Setup(m => m.Map<List<AppointmentDTOGet>>(appointments))
                .Returns(expectedDtos);

            // Act
            var result = await _service.GetAllAppointments();

            // Assert
            Assert.Equal(expectedDtos, result);
        }

        [Fact]
        public async Task DeleteAppointmentById_ValidId_ShouldCallRepository()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();

            // Act
            await _service.DeleteAppointmentById(appointmentId);

            // Assert
            _mockAppointmentRepo.Verify(r => r.DeleteAppointmentById(appointmentId), Times.Once);
        }

        [Fact]
        public async Task AppointmentExists_ShouldReturnRepositoryResult()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            _mockAppointmentRepo.Setup(r => r.AppointmentExists(appointmentId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.AppointmentExists(appointmentId);

            // Assert
            Assert.True(result);
        }
    }
}