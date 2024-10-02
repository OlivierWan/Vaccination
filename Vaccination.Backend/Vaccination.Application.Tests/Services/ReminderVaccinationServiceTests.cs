using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Services;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;

namespace Vaccination.Application.Tests.Services
{
    [TestFixture]
    public class ReminderVaccinationServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<UserManager<User>> _userManagerMock;
        private IReminderVaccinationService _reminderVaccinationService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<User>>(),
                Array.Empty<IUserValidator<User>>(),
                Array.Empty<IPasswordValidator<User>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>()
            );
            _reminderVaccinationService = new ReminderVaccinationService(_unitOfWorkMock.Object, _userManagerMock.Object);
        }

        [Test]
        public void GetUpcomingRemindersAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "nonexistentUser";
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _reminderVaccinationService.GetUpcomingRemindersAsync(userId));
        }

        [Test]
        public async Task GetUpcomingRemindersAsync_UserWithDateOfBirth_ReturnsReminders()
        {
            // Arrange
            var userId = "user1";
            var user = new User { Id = userId, DateOfBirth = new DateOnly(2020, 1, 1), FirstName = "John", LastName = "Doe" };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var calendarVaccinations = new List<CalendarVaccination>
            {
                new() { Id = Guid.NewGuid(), Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 },
                new() { Id = Guid.NewGuid(), Name = "Vaccine2", Description = "Desc2", MonthAge = 24, MonthDelay = 0 }
            };
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetAllAsync()).ReturnsAsync(calendarVaccinations);
            _unitOfWorkMock.Setup(uow => uow.UserVaccinations.GetUserVaccinationsByUserIdAsync(userId)).ReturnsAsync([]);

            // Act
            var result = await _reminderVaccinationService.GetUpcomingRemindersAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetUpcomingRemindersAsync_UserWithoutDateOfBirth_ReturnsReminders()
        {
            // Arrange
            var userId = "user2";
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var userVaccinations = new List<UserVaccination>
            {
                new() {
                    UserId = userId,
                    VaccineCalendarId = Guid.NewGuid(),
                    VaccinationDate = new DateOnly(2021, 1, 1),
                    VaccineCalendar = new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine1", Description = "Desc1", MonthAge = 12, MonthDelay = 0 },
                    User = user
                }
            };
            _unitOfWorkMock.Setup(uow => uow.UserVaccinations.GetUserVaccinationsByUserIdAsync(userId)).ReturnsAsync(userVaccinations);

            var calendarVaccinations = new List<CalendarVaccination>
            {
                new() { Id = Guid.NewGuid(), Name = "Vaccine2", Description = "Desc2", MonthAge = 24, MonthDelay = 0 }
            };
            _unitOfWorkMock.Setup(uow => uow.CalendarVaccinations.GetAllAsync()).ReturnsAsync(calendarVaccinations);

            // Act
            var result = await _reminderVaccinationService.GetUpcomingRemindersAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(1).Items);
        }

        [TestCase("2020-01-01", "2021-01-01", 12)]
        [TestCase("2020-01-01", "2022-01-01", 24)]
        [TestCase("2020-01-01", "2021-06-01", 17)]
        [TestCase("2020-06-01", "2021-01-01", 7)]
        [TestCase("2020-01-01", "2020-12-31", 11)]
        public void CalculateAgeInMonths_ValidDates_ReturnsCorrectAge(string birthDateString, string currentDateString, int expectedMonths)
        {
            // Arrange
            var formatProvider = System.Globalization.CultureInfo.InvariantCulture;
            DateOnly birthDate = DateOnly.Parse(birthDateString, formatProvider);
            DateOnly currentDate = DateOnly.Parse(currentDateString, formatProvider);

            // Act
            int result = ReminderVaccinationService.CalculateAgeInMonths(birthDate, currentDate);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMonths));
        }
    }
}