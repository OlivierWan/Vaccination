using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Exceptions;
using Vaccination.Infrastructure.Interceptors;
using Vaccination.Infrastructure.Repositories;

namespace Vaccination.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class UserVaccinationRepositoryTests
    {
        private DbContextOptions<VaccinationContext> _dbContextOptions;
        private VaccinationContext _context;
        private UserVaccinationRepository _repository;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private UpdateAuditableEntitiesInterceptor _updateAuditableEntitiesInterceptor;

        [SetUp]
        public void SetUp()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _updateAuditableEntitiesInterceptor = new UpdateAuditableEntitiesInterceptor(_httpContextAccessorMock.Object);

            _dbContextOptions = new DbContextOptionsBuilder<VaccinationContext>()
                .UseInMemoryDatabase(databaseName: "VaccinationTestDb")
                .AddInterceptors(_updateAuditableEntitiesInterceptor)
                .Options;

            _context = new VaccinationContext(_dbContextOptions);
            _repository = new UserVaccinationRepository(_context);

            // Seed the database with test data
            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var user = new User { Id = "user1", FirstName = "John", LastName = "Doe" };
            var calendarVaccination = new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine A", Description = "Description A", MonthAge = 2, MonthDelay = 1 };
            var userVaccination = new UserVaccination
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                VaccineCalendarId = calendarVaccination.Id,
                VaccinationDate = DateOnly.FromDateTime(DateTime.UtcNow),
                User = user,
                VaccineCalendar = calendarVaccination
            };

            _context.Users.Add(user);
            _context.CalendarVaccinations.Add(calendarVaccination);
            _context.UserVaccinations.Add(userVaccination);
            _context.SaveChanges();
        }

        [Test]
        public async Task CreateVaccinationAsync_AddsNewVaccination()
        {
            // Arrange
            var user = await _context.Users.FirstAsync(u => u.Id == "user1");
            var vaccineCalendar = await _context.CalendarVaccinations.FirstAsync();

            // Ensure the new vaccination has a unique VaccineCalendarId
            var newVaccineCalendar = new CalendarVaccination
            {
                Id = Guid.NewGuid(),
                Name = "Vaccine B",
                Description = "Description B",
                MonthAge = 3,
                MonthDelay = 1
            };
            _context.CalendarVaccinations.Add(newVaccineCalendar);
            await _context.SaveChangesAsync();

            var newVaccination = new UserVaccination
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                VaccineCalendarId = newVaccineCalendar.Id,
                VaccinationDate = DateOnly.FromDateTime(DateTime.UtcNow),
                User = user,
                VaccineCalendar = newVaccineCalendar
            };

            // Act
            await _repository.CreateVaccinationAsync(newVaccination);

            // Assert
            var result = await _context.UserVaccinations.FindAsync(newVaccination.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetUserVaccinationByIdAsync_ReturnsCorrectVaccination()
        {
            // Arrange
            var vaccination = await _context.UserVaccinations.FirstAsync();

            // Act
            var result = await _repository.GetUserVaccinationByIdAsync(vaccination.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(vaccination.Id));
        }

        [Test]
        public async Task GetUserVaccinationsByUserIdAsync_ReturnsCorrectVaccinations()
        {
            // Act
            var result = await _repository.GetUserVaccinationsByUserIdAsync("user1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateVaccinationAsync_UpdatesVaccination()
        {
            // Arrange
            var vaccination = await _context.UserVaccinations.FirstAsync();
            vaccination.Description = "Updated Description";

            // Act
            await _repository.UpdateVaccinationAsync(vaccination);

            // Assert
            var result = await _context.UserVaccinations.FindAsync(vaccination.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Description, Is.EqualTo("Updated Description"));
        }
    }
}