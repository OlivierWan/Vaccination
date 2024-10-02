using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Vaccination.Application.Dtos.User;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Services;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;

namespace Vaccination.Application.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private IUserService _userService;

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
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userManagerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task DeleteUserAsync_ValidRequest_ReturnsTrue()
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest("userId");
            var user = new User { Id = "userId", FirstName = "John", LastName = "Doe" };

            _userManagerMock.Setup(u => u.FindByIdAsync(deleteUserRequest.UserId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.DeleteUserAsync(deleteUserRequest);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteUserAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest("userId");

            _userManagerMock.Setup(u => u.FindByIdAsync(deleteUserRequest.UserId)).ReturnsAsync((User?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.DeleteUserAsync(deleteUserRequest));
        }

        [Test]
        public void DeleteUserAsync_DeleteFailed_ThrowsDatabaseException()
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest("userId");
            var user = new User { Id = "userId", FirstName = "John", LastName = "Doe" };
            var identityErrors = new[] { new IdentityError { Description = "Error" } };

            _userManagerMock.Setup(u => u.FindByIdAsync(deleteUserRequest.UserId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act & Assert
            Assert.ThrowsAsync<DatabaseException>(async () => await _userService.DeleteUserAsync(deleteUserRequest));
        }

        [Test]
        public async Task GetUserDetails_ValidRequest_ReturnsUserDetailsResponse()
        {
            // Arrange
            var userDetailsRequest = new UserDetailsRequest("userId");
            var user = new User { Id = "userId", FirstName = "John", LastName = "Doe" };
            var userDetailsResponse = new UserDetailsResponse(Guid.NewGuid(), "FirstName", "LastName", "Email", null, null, null, null, null, null, null);

            _userManagerMock.Setup(u => u.FindByIdAsync(userDetailsRequest.UserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDetailsResponse>(user)).Returns(userDetailsResponse);

            // Act
            var result = await _userService.GetUserDetails(userDetailsRequest);

            // Assert
            Assert.That(result, Is.EqualTo(userDetailsResponse));
        }

        [Test]
        public void GetUserDetails_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var userDetailsRequest = new UserDetailsRequest("userId");

            _userManagerMock.Setup(u => u.FindByIdAsync(userDetailsRequest.UserId)).ReturnsAsync((User?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetUserDetails(userDetailsRequest));
        }

        [Test]
        public async Task UpdateUserDetails_ValidRequest_ReturnsUpdateUserResponse()
        {
            // Arrange
            var userId = "userId";
            var updateUserRequest = new UpdateUserRequest("FirstName", "LastName", "Email", null, null, null, null, null, null, null);
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            var updateUserResponse = new UpdateUserResponse();

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map(updateUserRequest, user));
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _mapperMock.Setup(m => m.Map<UpdateUserResponse>(user)).Returns(updateUserResponse);

            // Act
            var result = await _userService.UpdateUserDetails(userId, updateUserRequest);

            // Assert
            Assert.That(result, Is.EqualTo(updateUserResponse));
        }

        [Test]
        public void UpdateUserDetails_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "userId";
            var updateUserRequest = new UpdateUserRequest("FirstName", "LastName", "Email", null, null, null, null, null, null, null);

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.UpdateUserDetails(userId, updateUserRequest));
        }

        [Test]
        public void UpdateUserDetails_UpdateFailed_ThrowsDatabaseException()
        {
            // Arrange
            var userId = "userId";
            var updateUserRequest = new UpdateUserRequest("FirstName", "LastName", "Email", null, null, null, null, null, null, null);
            var user = new User { Id = userId , FirstName = "John", LastName = "Doe" };
            var identityErrors = new[] { new IdentityError { Description = "Error" } };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act & Assert
            Assert.ThrowsAsync<DatabaseException>(async () => await _userService.UpdateUserDetails(userId, updateUserRequest));
        }
    }
}
