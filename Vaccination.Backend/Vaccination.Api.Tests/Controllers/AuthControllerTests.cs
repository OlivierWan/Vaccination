using Microsoft.AspNetCore.Mvc;
using Moq;
using Vaccination.Api.Controllers;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Authentication;
using Vaccination.Application.Interfaces;

namespace Vaccination.Api.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _authController = new AuthController(_authServiceMock.Object);
        }

        [Test]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new LoginRequest("test@example.com", "passwordpassword");
            var loginResponse = new LoginResponse(DateTime.UtcNow.AddHours(1), "refreshToken", "token");

            _authServiceMock.Setup(x => x.LoginAsync(loginRequest)).ReturnsAsync(loginResponse);

            // Act
            var result = await _authController.Login(loginRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            var response = result.Value as ApiResponse<LoginResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Connexion réussie"));
            Assert.That(response.Data, Is.EqualTo(loginResponse));
        }

        [Test]
        public async Task RefreshToken_ShouldReturnOk_WhenTokenIsRefreshed()
        {
            // Arrange
            var tokenRequest = new TokenRequest("accessToken", "refreshToken");
            var tokenResponse = new TokenResponse("newRefreshToken", "newToken", DateTime.UtcNow.AddHours(1));

            _authServiceMock.Setup(x => x.RefreshTokenAsync(tokenRequest)).ReturnsAsync(tokenResponse);

            // Act
            var result = await _authController.RefreshToken(tokenRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            var response = result.Value as ApiResponse<TokenResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("JWT Token rafraîchi avec succès"));
            Assert.That(response.Data, Is.EqualTo(tokenResponse));
        }

        [Test]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerRequest = new RegisterRequest("test@example.com", "FirstName", "LastName", "passwordpassword");
            var registerResponse = new RegisterResponse();

            _authServiceMock.Setup(x => x.RegisterAsync(registerRequest)).ReturnsAsync(registerResponse);

            // Act
            var result = await _authController.Register(registerRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            var response = result.Value as ApiResponse<RegisterResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Utilisateur enregistré avec succès"));
            Assert.That(response.Data, Is.EqualTo(registerResponse));
        }
    }
}