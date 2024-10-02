using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Vaccination.Api.Controllers;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Reminder;
using Vaccination.Application.Interfaces;

namespace Vaccination.Api.Tests.Controllers
{
    [TestFixture]
    public class ReminderControllerTests
    {
        private Mock<IReminderVaccinationService> _reminderServiceMock;
        private ReminderController _reminderController;

        [SetUp]
        public void Setup()
        {
            _reminderServiceMock = new Mock<IReminderVaccinationService>();
            _reminderController = new ReminderController(_reminderServiceMock.Object);

            // Create a mock ClaimsPrincipal
            var userClaims = new List<Claim>
            {
                new Claim("userid", "test-user-id")
            };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Set the User property of the controller
            _reminderController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task GetUpcomingRemindersAsync_ReturnsOkResult_WithUpcomingReminders()
        {
            // Arrange
            var upcomingReminders = new List<ReminderVaccinationResponse>
            {
                new ReminderVaccinationResponse("Reminder1", "Description1", "Message1", 6, Guid.NewGuid()),
                new ReminderVaccinationResponse("Reminder2", "Description2", "Message2", 12, Guid.NewGuid())
            };
            _reminderServiceMock.Setup(service => service.GetUpcomingRemindersAsync(It.IsAny<string>()))
                                .ReturnsAsync(upcomingReminders);

            // Act
            var result = await _reminderController.GetUpcomingRemindersAsync();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<IEnumerable<ReminderVaccinationResponse>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Rappels à venir récupérés avec succès"));
            Assert.That(response?.Data, Is.EqualTo(upcomingReminders));
        }
    }
}