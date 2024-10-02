using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vaccination.Api.Exceptions;

namespace Vaccination.Api.Tests.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private TestableBaseController _baseController;

        [SetUp]
        public void Setup()
        {
            _baseController = new TestableBaseController();
        }

        [Test]
        public void GetUserId_ReturnsUserId_WhenUserIdIsPresent()
        {
            // Arrange
            var userClaims = new List<Claim>
            {
                new Claim("userid", "test-user-id")
            };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _baseController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var userId = _baseController.GetUserId();

            // Assert
            Assert.That(userId, Is.EqualTo("test-user-id"));
        }

        [Test]
        public void GetUserId_ThrowsSecurityTokenException_WhenUserIdIsNotPresent()
        {
            // Arrange
            var userClaims = new List<Claim>();
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _baseController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act & Assert
            var ex = Assert.Throws<SecurityTokenException>(() => _baseController.GetUserId());
            Assert.That(ex.Message, Is.EqualTo("UserId not found in the token"));
        }
    }
}