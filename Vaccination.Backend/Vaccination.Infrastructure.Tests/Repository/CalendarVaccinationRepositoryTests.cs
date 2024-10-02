using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Domain.Entities;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Interceptors;
using Vaccination.Infrastructure.Repositories;

namespace Vaccination.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class CalendarVaccinationRepositoryTests
    {
        private DbContextOptions<VaccinationContext> _dbContextOptions;
        private VaccinationContext _context;
        private CalendarVaccinationRepository _repository;
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
            _repository = new CalendarVaccinationRepository(_context);

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
            var calendarVaccinations = new List<CalendarVaccination>
            {
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine A", Description = "Description A", MonthAge = 2, MonthDelay = 1 },
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine B", Description = "Description B", MonthAge = 4, MonthDelay = 1 },
                new CalendarVaccination { Id = Guid.NewGuid(), Name = "Vaccine C", Description = "Description C", MonthAge = 6, MonthDelay = 1 }
            };

            _context.CalendarVaccinations.AddRange(calendarVaccinations);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCalendarVaccinations()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetCalendarVaccinationByIdAsync_ReturnsCorrectVaccination()
        {
            // Arrange
            var vaccination = _context.CalendarVaccinations.First();

            // Act
            var result = await _repository.GetCalendarVaccinationByIdAsync(vaccination.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(vaccination.Id));
        }

        [Test]
        public async Task GetNextVaccinesAsync_ReturnsCorrectVaccinations()
        {
            // Arrange
            var userVaccins = new List<Guid> { _context.CalendarVaccinations.First().Id };

            // Act
            var result = await _repository.GetNextVaccinesAsync(userVaccins);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}