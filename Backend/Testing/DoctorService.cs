using AutoMapper;
using Hospital.Data.DTOs;
using Hospital.Data.Models;
using Hospital.Interfaces.Repos;
using Hospital.Services;
using Moq;
using Xunit;

namespace Hospital.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepo> _mockDocRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _mockDocRepo = new Mock<IDoctorRepo>();
            _mockMapper = new Mock<IMapper>();
            _doctorService = new DoctorService(_mockMapper.Object, _mockDocRepo.Object);
        }

        [Fact]
        public async Task DoctorExists_ReturnsTrue_WhenDoctorExists()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            _mockDocRepo.Setup(repo => repo.DoctorExists(doctorId)).ReturnsAsync(true);

            // Act
            var result = await _doctorService.DoctorExists(doctorId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddDoctor_CallsRepoWithMappedDoctor()
        {
            // Arrange
            var doctorDto = new DoctorDTOUpdate();
            var doctor = new Doctor
            {
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine"
            };
            _mockMapper.Setup(m => m.Map<Doctor>(doctorDto)).Returns(doctor);

            // Act
            await _doctorService.AddDoctor(doctorDto);

            // Assert
            _mockDocRepo.Verify(repo => repo.AddDoctor(doctor), Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorById_CallsRepoWithCorrectId()
        {
            // Arrange
            var doctorId = Guid.NewGuid();

            // Act
            await _doctorService.DeleteDoctorById(doctorId);

            // Assert
            _mockDocRepo.Verify(repo => repo.DeleteDoctorById(doctorId), Times.Once);
        }

        [Fact]
        public async Task FilterDoctors_ReturnsFilteredDoctors()
        {
            // Arrange
            var specialty = "Cardiology";
            var yearsOfExp = 5;
            var name = "Dr. Smith";

            var doctors = new List<Doctor>
            {              
            new Doctor { Specialty = "Cardiology", YearsOfExperience = 10, Password = "securepassword", DoctorName = "Dr. Smith", Email = "Smith.doe@example.com", },
                new Doctor { Specialty = "Neurology", YearsOfExperience = 8, Password = "securepassword", DoctorName = "Dr. Johnson", Email = "johnson.doe@example.com", }
            };

            var expectedDto = new List<DoctorDTOGet> { new DoctorDTOGet() };

            _mockDocRepo.Setup(repo => repo.GetAllDoctors()).ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<List<DoctorDTOGet>>(It.IsAny<List<Doctor>>())).Returns(expectedDto);

            // Act
            var result = await _doctorService.FilterDoctors(specialty, yearsOfExp, name);

            // Assert
            Assert.Single(result);
            _mockMapper.Verify(m => m.Map<List<DoctorDTOGet>>(It.Is<List<Doctor>>(d => d.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task GetAllAppointmentsByDoctorId_ReturnsMappedAppointments()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor
            {
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine",
                Appointments = new List<Appointment>
                {
                    new Appointment(),
                    new Appointment()
                }
            };

            var expectedDto = new List<AppointmentDTOGet> { new(), new() };

            _mockDocRepo.Setup(repo => repo.GetDoctorWithNavProp(doctorId)).ReturnsAsync(doctor);
            _mockMapper.Setup(m => m.Map<List<AppointmentDTOGet>>(doctor.Appointments)).Returns(expectedDto);

            // Act
            var result = await _doctorService.GetAllAppointmentsByDoctorId(doctorId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllUpcomingAppointmentsByDoctorId_ReturnsOnlyFutureAppointments()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var now = DateTime.Now;

            var appointments = new List<AppointmentDTOGet>
            {
                new() { AppointmentDate = now.AddDays(1) },
                new() { AppointmentDate = now.AddDays(-1) },
                new() { AppointmentDate = now.AddHours(1) }
            };

            _mockDocRepo.Setup(repo => repo.GetDoctorWithNavProp(doctorId)).ReturnsAsync(new Doctor() {
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine"
            });
            _mockMapper.Setup(m => m.Map<List<AppointmentDTOGet>>(It.IsAny<List<Appointment>>()))
                .Returns(appointments);

            // Act
            var result = await _doctorService.GetAllUpcomingAppointmentsByDoctorId(doctorId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.True(a.AppointmentDate > now));
        }

        [Fact]
        public async Task GetAllDoctors_ReturnsMappedDoctors()
        {
            // Arrange
 
            var doctors = new List<Doctor> { new Doctor
    {
        DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine"
    },new Doctor
    {
        DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine"
    } };
            var expectedDto = new List<DoctorDTOGet> { new(), new() };

            _mockDocRepo.Setup(repo => repo.GetAllDoctors()).ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<List<DoctorDTOGet>>(doctors)).Returns(expectedDto);

            // Act
            var result = await _doctorService.GetAllDoctors();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllPatientsByDoctorId_ReturnsMappedPatients()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor
            {
                            
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine",
            
            DoctorPatients = new List<DoctorPatient>
                {
                    new() { Patient = new Patient(){PatientName = "Jane Smith",
                Email = "jane.smith@example.com",
                Password = "AnotherSecurePassword",
                DateOfBirth = new DateTime(1985, 5, 10) } },
                    new() { Patient = new Patient(){PatientName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Password = "AnotherSecurePassword",
                    DateOfBirth = new DateTime(1985, 5, 10)  } }
                }
            };

            var expectedDto = new List<PatientDTOGet> { new(), new() };

            _mockDocRepo.Setup(repo => repo.GetDoctorWithNavProp(doctorId)).ReturnsAsync(doctor);
            _mockMapper.Setup(m => m.Map<List<PatientDTOGet>>(It.IsAny<List<Patient>>())).Returns(expectedDto);

            // Act
            var result = await _doctorService.GetAllPatientsByDoctorId(doctorId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetDoctorById_ReturnsMappedDoctor()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor
            {
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine"
            };
            var expectedDto = new DoctorDTOGet();

            _mockDocRepo.Setup(repo => repo.GetDoctorById(doctorId)).ReturnsAsync(doctor);
            _mockMapper.Setup(m => m.Map<DoctorDTOGet>(doctor)).Returns(expectedDto);

            // Act
            var result = await _doctorService.GetDoctorById(doctorId);

            // Assert
            Assert.Same(expectedDto, result);
        }

        [Fact]
        public async Task UpdateDoctorById_CallsRepoWithMappedDoctor()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctorDto = new DoctorDTOUpdate();
            var doctor = new Doctor
            {
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine"
            };

            _mockMapper.Setup(m => m.Map<Doctor>(doctorDto)).Returns(doctor);

            // Act
            await _doctorService.UpdateDoctorById(doctorId, doctorDto);

            // Assert
            _mockDocRepo.Verify(repo => repo.UpdateDoctorById(doctorId, doctor), Times.Once);
        }

        [Fact]
        public async Task GetDoctorRatingByPatientId_ReturnsRating_WhenExists()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var patientId = Guid.NewGuid();
            var expectedRating = 4.5;

            var doctor = new Doctor
            {
              
            
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine",
            
            DoctorPatients = new List<DoctorPatient>
                {
                    new() { PatientId = patientId, Rating = expectedRating }
                }
            };

            _mockDocRepo.Setup(repo => repo.GetDoctorWithNavProp(doctorId)).ReturnsAsync(doctor);

            // Act
            var result = await _doctorService.GetDoctorRatingByPatientId(doctorId, patientId);

            // Assert
            Assert.Equal(expectedRating, result);
        }

        [Fact]
        public async Task GetAverageDoctorRating_ReturnsAverage_WhenRatingsExist()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor
            {
                
            
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine",
            
            DoctorPatients = new List<DoctorPatient>
                {
                    new() { Rating = 4 },
                    new() { Rating = 5 },
                    new() { Rating = 0 } // should be ignored
                }
            };

            _mockDocRepo.Setup(repo => repo.GetDoctorWithNavProp(doctorId)).ReturnsAsync(doctor);

            // Act
            var result = await _doctorService.GetAverageDoctorRating(doctorId);

            // Assert
            Assert.Equal(4.5, result);
        }

        [Fact]
        public async Task GetProfit_CalculatesCorrectly()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var date = DateTime.Now;
            var hourlyPay = 100;

            var doctor = new Doctor
            {
               
            
                DoctorName = "Dr. John Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                Specialty = "General Medicine",
            
            HourlyPay = hourlyPay,
                Appointments = new List<Appointment>
                {
                    new() { AppointmentDate = date.AddDays(-1), Duration = 2 },
                    new() { AppointmentDate = date.AddHours(-1), Duration = 1 },
                    new() { AppointmentDate = date.AddDays(1), Duration = 3 } // should be ignored
                }
            };

            _mockDocRepo.Setup(repo => repo.GetDoctorWithNavProp(doctorId)).ReturnsAsync(doctor);

            // Act
            var result = await _doctorService.GetProfit(doctorId, date);

            // Assert
            Assert.Equal(300, result); // (2 + 1) * 100
        }

        [Fact]
        public async Task GetAllReviewsForDoctorById_ReturnsMappedReviews()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var reviews = new List<DoctorPatient> { new(), new() };
            var expectedDto = new List<DoctorPatientDTO> { new(), new() };

            _mockDocRepo.Setup(repo => repo.GetAllReviewsForDoctorById(doctorId)).ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<List<DoctorPatientDTO>>(reviews)).Returns(expectedDto);

            // Act
            var result = await _doctorService.GetAllReviewsForDoctorById(doctorId);

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}