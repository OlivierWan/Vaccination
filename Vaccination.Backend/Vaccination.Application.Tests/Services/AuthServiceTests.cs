using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Vaccination.Application.Dtos.Authentication;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Services;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;

namespace Vaccination.Application.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IAuthService _authService;

        [SetUp]
        public void SetUp()
        {
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
            _configurationMock = new Mock<IConfiguration>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _authService = new AuthService(_userManagerMock.Object, _configurationMock.Object, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task LoginAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var loginRequest = new LoginRequest("test@example.com", "password");
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            // Act & Assert
            await Task.Run(() => Assert.ThrowsAsync<NotFoundException>(async () => await _authService.LoginAsync(loginRequest)));
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ThrowsPasswordException()
        {
            // Arrange
            var loginRequest = new LoginRequest("test@example.com", "password");
            var user = new User { Email = "test@example.com", FirstName = "Test", LastName = "User" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act & Assert
            await Task.Run(() => Assert.ThrowsAsync<PasswordException>(async () => await _authService.LoginAsync(loginRequest)));
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest("test@example.com", "password");
            var user = new User { Email = "test@example.com", Id = "1", FirstName = "Test", LastName = "User" };

            // Set the JWT_SECRET environment variable
            Environment.SetEnvironmentVariable("JWT_SECRET", Guid.NewGuid().ToString());

            // Mock the configuration values
            _configurationMock.Setup(c => c["JWT:TokenValidity"]).Returns("1"); // 1 hour validity
            _configurationMock.Setup(c => c["JWT:ValidIssuer"]).Returns("your_issuer");
            _configurationMock.Setup(c => c["JWT:ValidAudience"]).Returns("your_audience");
            _configurationMock.Setup(c => c["JWT:RefreshToken"]).Returns("7");

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(["User"]);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Token, Is.Not.Null);
                Assert.That(result.RefreshToken, Is.Not.Null);
            });
        }

        [Test]
        public async Task RegisterAsync_UserAlreadyExists_ThrowsDuplicateDataException()
        {
            // Arrange
            var registerRequest = new RegisterRequest("test@example.com", "FirstName", "LastName", "password");
            var existingUser = new User { Email = "test@example.com", FirstName = "Test", LastName = "User" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

            // Act & Assert
            await Task.Run(() => Assert.ThrowsAsync<DuplicateDataException>(async () => await _authService.RegisterAsync(registerRequest)));
        }

        [Test]
        public async Task RegisterAsync_ValidRequest_ReturnsRegisterResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest("test@example.com", "FirstName", "LastName", "password");
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterAsync(registerRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}