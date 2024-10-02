using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Vaccination.Api.Controllers;
using Vaccination.Application.Dtos;
using Vaccination.Application.Dtos.Vaccination;
using Vaccination.Application.Interfaces;
using Vaccination.Domain.Shared;

namespace Vaccination.Api.Tests.Controllers
{
    [TestFixture]
    public class VaccinationControllerTests
    {
        private Mock<IUserVaccinationService> _vaccinationServiceMock;
        private VaccinationController _vaccinationController;

        [SetUp]
        public void Setup()
        {
            _vaccinationServiceMock = new Mock<IUserVaccinationService>();
            _vaccinationController = new VaccinationController(_vaccinationServiceMock.Object);

            // Create a mock ClaimsPrincipal
            var userClaims = new List<Claim>
            {
                new Claim("userid", "test-user-id")
            };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Set the User property of the controller
            _vaccinationController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task Create_ReturnsCreatedResult_WithCreatedVaccination()
        {
            // Arrange
            var createRequest = new CreateUserVaccinationRequest(DateOnly.FromDateTime(DateTime.Now), "Description", Guid.NewGuid());
            var createdResponse = new CreatedUserVaccinationResponse(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), "Description", Guid.NewGuid());

            _vaccinationServiceMock.Setup(service => service.CreateVaccinationAsync(createRequest, It.IsAny<string>()))
                                   .ReturnsAsync(createdResponse);

            // Act
            var result = await _vaccinationController.Create(createRequest);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            var response = createdResult?.Value as ApiResponse<CreatedUserVaccinationResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Vaccination créée avec succès"));
            Assert.That(response?.Data, Is.EqualTo(createdResponse));
        }

        [Test]
        public async Task Delete_ReturnsOkResult_WithDeletionStatus()
        {
            // Arrange
            var vaccinationId = Guid.NewGuid();
            _vaccinationServiceMock.Setup(service => service.DeleteVaccinationAsync(vaccinationId))
                                   .ReturnsAsync(true);

            // Act
            var result = await _vaccinationController.Delete(vaccinationId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<bool>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Vaccination supprimée avec succès"));
            Assert.That(response?.Data, Is.True);
        }

        [Test]
        public async Task Update_ReturnsOkResult_WithUpdatedVaccination()
        {
            // Arrange
            var updateRequest = new UpdateUserVaccinationRequest(DateOnly.FromDateTime(DateTime.Now), "Updated Description");
            var updateResponse = new UpdateUserVaccinationResponse(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), "Updated Description", Guid.NewGuid());

            _vaccinationServiceMock.Setup(service => service.UpdateVaccinationAsync(It.IsAny<Guid>(), updateRequest))
                                   .ReturnsAsync(updateResponse);

            // Act
            var result = await _vaccinationController.Update(updateResponse.Id, updateRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<UpdateUserVaccinationResponse>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Vaccination mise à jour avec succès"));
            Assert.That(response?.Data, Is.EqualTo(updateResponse));
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithVaccinations()
        {
            // Arrange
            var getRequest = new GetFilteredUserVaccinationRequest(null, "OrderBy", "OrderDirection");
            var pagedList = new PagedList<UserVaccinationResponse>(new List<UserVaccinationResponse>
            {
                new UserVaccinationResponse(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid(), "Vaccine1", "Description1"),
                new UserVaccinationResponse(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid(), "Vaccine2", "Description2")
            }, 2, 1, 50);

            _vaccinationServiceMock.Setup(service => service.GetAllUserVaccinationsAsync(getRequest, It.IsAny<string>()))
                                   .ReturnsAsync(pagedList);

            // Act
            var result = await _vaccinationController.GetAll(getRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult?.Value as ApiResponse<PagedList<UserVaccinationResponse>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response?.Message, Is.EqualTo("Vaccinations récupérées avec succès"));
            Assert.That(response?.Data, Is.EqualTo(pagedList));
        }
    }
}