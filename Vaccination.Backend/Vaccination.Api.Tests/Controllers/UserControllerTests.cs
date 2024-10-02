using Microsoft.AspNetCore.Mvc;
using Moq;
using Vaccination.Api.Controllers;
using Vaccination.Application.Dtos.User;
using Vaccination.Application.Dtos;
using Vaccination.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Vaccination.Api.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);

            // Create a mock ClaimsPrincipal
            var userClaims = new List<Claim>
            {
                new("userid", "test-user-id")
            };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Set the User property of the controller
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task GetUser_ReturnsOkResult_WithUserDetails()
        {
            // Arrange
            var userDetails = new UserDetailsResponse(
                Guid.NewGuid(), "John", "Doe", "john.doe@example.com", null, null, "City", "Nationality", "Address", "PostalCode", "PhoneNumber"
            );
            _userServiceMock.Setup(service => service.GetUserDetails(It.IsAny<UserDetailsRequest>()))
                            .ReturnsAsync(userDetails);

            // Act
            var result = await _userController.GetUser();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<UserDetailsResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response?.Message, Is.EqualTo("Détails de l'utilisateur récupérés avec succès"));
                Assert.That(response?.Data, Is.EqualTo(userDetails));
            });
        }

        [Test]
        public async Task UpdateUser_ReturnsOkResult_WithUpdateUserResponse()
        {
            // Arrange
            var updateUserRequest = new UpdateUserRequest("John", "Doe", "john.doe@example.com", null, null, "City", "Nationality", "Address", "PostalCode", "PhoneNumber");
            var updateUserResponse = new UpdateUserResponse();
            _userServiceMock.Setup(service => service.UpdateUserDetails(It.IsAny<string>(), updateUserRequest))
                            .ReturnsAsync(updateUserResponse);

            // Act
            var result = await _userController.UpdateUser(updateUserRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<UpdateUserResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response?.Message, Is.EqualTo("Utilisateur mis à jour avec succès"));
                Assert.That(response?.Data, Is.EqualTo(updateUserResponse));
            });
        }

        [Test]
        public async Task DeleteAccount_ReturnsOkResult_WithDeleteUserResponse()
        {
            // Arrange
            _userServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<DeleteUserRequest>()))
                            .ReturnsAsync(true);

            // Act
            var result = await _userController.DeleteAccount();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<DeleteUserResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response?.Message, Is.EqualTo("Utilisateur supprimé avec succès"));
                Assert.That(response?.Data?.IsDeleted, Is.True);
            });
        }
    }
}