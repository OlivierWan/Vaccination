using AutoMapper;
using Moq;
using Vaccination.Application.Dtos.Calendar;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Services;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Tests.Services
{
    [TestFixture]
    public class CalendarVaccinationServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private ICalendarVaccinationService _calendarVaccinationService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _calendarVaccinationService = new CalendarVaccinationService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedCalendarVaccinationResponses()
        {
            // Arrange
            var calendarVaccinations = new List<CalendarVaccination>
            {
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 },
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine2", Description = "Desc2", MonthAge = 24, MonthDelay = 1 }
            };
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetAllAsync()).ReturnsAsync(calendarVaccinations);
            _mapperMock.Setup(m => m.Map<IEnumerable<CalendarVaccinationResponse>>(calendarVaccinations))
                       .Returns(new List<CalendarVaccinationResponse>
                       {
                           new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine1", "Desc1", 12, 0),
                           new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine2", "Desc2", 24, 1)
                       });

            // Act
            var result = await _calendarVaccinationService.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetAllWithPaginationAsync_ReturnsMappedPagedList()
        {
            // Arrange
            var request = new GetFilteredCalendarVaccinationRequest(null, "Name", "asc", 1, 10);
            var pagedList = new PagedList<CalendarVaccination>(new List<CalendarVaccination>
            {
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 }
            }, 1, 1, 10);
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetAllCalendarVaccinationsAsync(request.PageNumber, request.PageSize, request.CriteriaSearch))
                           .ReturnsAsync(pagedList);
            _mapperMock.Setup(m => m.Map<PagedList<CalendarVaccinationResponse>>(pagedList))
                       .Returns(new PagedList<CalendarVaccinationResponse>(new List<CalendarVaccinationResponse>
                       {
                           new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine1", "Desc1", 12, 0)
                       }, 1, 1, 10));

            // Act
            var result = await _calendarVaccinationService.GetAllWithPaginationAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(1).Items);
        }

        [Test]
        public async Task GetByIdAsync_CalendarVaccinationExists_ReturnsMappedResponse()
        {
            // Arrange
            var request = new GetCalendarVaccinationByIdRequest(Guid.NewGuid());
            var calendarVaccination = new CalendarVaccination { Id = request.Id, Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 };
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetCalendarVaccinationByIdAsync(request.Id))
                           .ReturnsAsync(calendarVaccination);
            _mapperMock.Setup(m => m.Map<CalendarVaccinationResponse>(calendarVaccination))
                       .Returns(new CalendarVaccinationResponse(request.Id, "Vaccine1", "Desc1", 12, 0));

            // Act
            var result = await _calendarVaccinationService.GetByIdAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(request.Id));
        }

        [Test]
        public void GetByIdAsync_CalendarVaccinationDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var request = new GetCalendarVaccinationByIdRequest(Guid.NewGuid());
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetCalendarVaccinationByIdAsync(request.Id))
                           .ReturnsAsync((CalendarVaccination?)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _calendarVaccinationService.GetByIdAsync(request));
        }

        [Test]
        public async Task GetNextVaccines_ReturnsMappedCalendarVaccinationResponses()
        {
            // Arrange
            var userId = "user1";
            var userVaccinations = new List<UserVaccination>
            {
                new UserVaccination
                {
                    UserId = userId,
                    VaccineCalendarId = Guid.NewGuid(),
                    User = new User { Id = userId, FirstName = "John", LastName = "Doe" }, // Set required User
                    VaccineCalendar = new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 }
                }
            };
            var calendarVaccinations = new List<CalendarVaccination>
            {
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 }
            };
            _unitOfWorkMock.Setup(uow => uow.UserVaccinations.GetUserVaccinationsByUserIdAsync(userId))
                           .ReturnsAsync(userVaccinations);
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetNextVaccinesAsync(It.IsAny<IEnumerable<Guid>>()))
                           .ReturnsAsync(calendarVaccinations);
            _mapperMock.Setup(m => m.Map<IEnumerable<CalendarVaccinationResponse>>(calendarVaccinations))
                       .Returns(new List<CalendarVaccinationResponse>
                       {
                           new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine1", "Desc1", 12, 0)
                       });

            // Act
            var result = await _calendarVaccinationService.GetNextVaccines(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(1).Items);
        }
    }
}