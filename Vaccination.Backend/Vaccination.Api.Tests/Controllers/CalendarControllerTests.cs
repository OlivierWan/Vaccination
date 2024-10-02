using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Vaccination.Api.Controllers;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Calendar;
using Vaccination.Application.Interfaces;

namespace Vaccination.Api.Tests.Controllers
{
    [TestFixture]
    public class CalendarControllerTests
    {
        private Mock<ICalendarVaccinationService> _calendarServiceMock;
        private CalendarController _calendarController;

        [SetUp]
        public void Setup()
        {
            _calendarServiceMock = new Mock<ICalendarVaccinationService>();
            _calendarController = new CalendarController(_calendarServiceMock.Object);

            // Create a mock ClaimsPrincipal
            var userClaims = new List<Claim>
            {
                new Claim("userid", "test-user-id")
            };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Set the User property of the controller
            _calendarController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithCalendarVaccinations()
        {
            // Arrange
            var calendarVaccinations = new List<CalendarVaccinationResponse>
            {
                new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine1", "Description1", 6, 0),
                new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine2", "Description2", 12, 0)
            };
            _calendarServiceMock.Setup(service => service.GetAllAsync())
                                .ReturnsAsync(calendarVaccinations);

            // Act
            var result = await _calendarController.GetAll(false);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<IEnumerable<CalendarVaccinationResponse>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Calendrier des vaccinations récupéré avec succès"));
            Assert.That(response?.Data, Is.EqualTo(calendarVaccinations));
        }

        [Test]
        public async Task GetAllNext_ReturnsOkResult_WithNextCalendarVaccinations()
        {
            // Arrange
            var nextVaccinations = new List<CalendarVaccinationResponse>
            {
                new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine3", "Description3", 18, 0),
                new CalendarVaccinationResponse(Guid.NewGuid(), "Vaccine4", "Description4", 24, 0)
            };
            _calendarServiceMock.Setup(service => service.GetNextVaccines(It.IsAny<string>()))
                                .ReturnsAsync(nextVaccinations);

            // Act
            var result = await _calendarController.GetAll(true);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<IEnumerable<CalendarVaccinationResponse>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Calendrier des vaccinations récupéré avec succès"));
            Assert.That(response?.Data, Is.EqualTo(nextVaccinations));
        }
    }
}