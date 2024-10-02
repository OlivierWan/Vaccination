using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Application.Dtos.Vaccination;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Application.Services;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Tests.Services
{
    [TestFixture]
    public class UserVaccinationServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IUserVaccinationService _userVaccinationService;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userVaccinationService = new UserVaccinationService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task CreateVaccinationAsync_ValidRequest_ReturnsCreatedUserVaccinationResponse()
        {
            // Arrange
            var createUserVaccinationRequest = new CreateUserVaccinationRequest(DateOnly.FromDateTime(DateTime.Now), "Description", Guid.NewGuid());
            var userVaccination = new UserVaccination
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                CreatedOnUtc = DateTime.UtcNow,
                UserId = "userId", // Set a valid user ID
                User = new User { FirstName = "John", LastName = "Doe" },
                VaccineCalendar = new CalendarVaccination { Description = "description", MonthAge = 1, MonthDelay = 0, Name = "Vaccine1" }
            };
            var createdUserVaccinationResponse = new CreatedUserVaccinationResponse(userVaccination.Id, userVaccination.VaccinationDate, userVaccination.Description, userVaccination.VaccineCalendarId);

            _mapperMock.Setup(m => m.Map<UserVaccination>(createUserVaccinationRequest)).Returns(userVaccination);
            _unitOfWorkMock.Setup(u => u.UserVaccinations.CreateVaccinationAsync(userVaccination)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CreatedUserVaccinationResponse>(userVaccination)).Returns(createdUserVaccinationResponse);

            // Act
            var result = await _userVaccinationService.CreateVaccinationAsync(createUserVaccinationRequest, "userId");

            // Assert
            Assert.That(result, Is.EqualTo(createdUserVaccinationResponse));
        }

        [Test]
        public async Task DeleteVaccinationAsync_ValidRequest_ReturnsTrue()
        {
            // Arrange
            var vaccinationId = Guid.NewGuid();
            var userVaccination = new UserVaccination
            {
                Id = vaccinationId,
                CreatedBy = Guid.NewGuid(),
                CreatedOnUtc = DateTime.UtcNow,
                UserId = "userId", // Set a valid user ID
                User = new User { FirstName = "John", LastName = "Doe" },
                VaccineCalendar = new CalendarVaccination { Description = "description", MonthAge = 1, MonthDelay = 0, Name = "Vaccine1" }
            };

            _unitOfWorkMock.Setup(u => u.UserVaccinations.GetUserVaccinationByIdAsync(vaccinationId)).ReturnsAsync(userVaccination);
            _unitOfWorkMock.Setup(u => u.UserVaccinations.DeleteVaccinationAsync(userVaccination)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _userVaccinationService.DeleteVaccinationAsync(vaccinationId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteVaccinationAsync_InvalidRequest_ThrowsNotFoundException()
        {
            // Arrange
            var vaccinationId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.UserVaccinations.GetUserVaccinationByIdAsync(vaccinationId)).ReturnsAsync((UserVaccination?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userVaccinationService.DeleteVaccinationAsync(vaccinationId));
        }

        [Test]
        public async Task GetAllUserVaccinationsAsync_ValidRequest_ReturnsPagedList()
        {
            // Arrange
            var getFilteredUserVaccinationRequest = new GetFilteredUserVaccinationRequest(null, "OrderBy", "OrderDirection", 1, 50);
            var userVaccinations = new PagedList<UserVaccination>(new List<UserVaccination>(), 0, 1, 50);
            var userVaccinationResponses = new PagedList<UserVaccinationResponse>(new List<UserVaccinationResponse>(), 0, 1, 50);

            _unitOfWorkMock.Setup(u => u.UserVaccinations.GetAllUserVaccinationsAsync(
                getFilteredUserVaccinationRequest.OrderBy,
                getFilteredUserVaccinationRequest.OrderDirection,
                getFilteredUserVaccinationRequest.PageNumber,
                getFilteredUserVaccinationRequest.PageSize,
                getFilteredUserVaccinationRequest.CriteriaSearch,
                "userId")).ReturnsAsync(userVaccinations);

            _mapperMock.Setup(m => m.Map<PagedList<UserVaccinationResponse>>(userVaccinations)).Returns(userVaccinationResponses);

            // Act
            var result = await _userVaccinationService.GetAllUserVaccinationsAsync(getFilteredUserVaccinationRequest, "userId");

            // Assert
            Assert.That(result, Is.EqualTo(userVaccinationResponses));
        }

        [Test]
        public async Task UpdateVaccinationAsync_ValidRequest_ReturnsUpdateUserVaccinationResponse()
        {
            // Arrange
            var updateUserVaccinationRequest = new UpdateUserVaccinationRequest(DateOnly.FromDateTime(DateTime.Now), "Description");
            var userVaccination = new UserVaccination
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                CreatedOnUtc = DateTime.UtcNow,
                UserId = "userId", // Set a valid user ID
                User = new User { FirstName = "John", LastName = "Doe" },
                VaccineCalendar = new CalendarVaccination { Description = "description", MonthAge = 1, MonthDelay = 0, Name = "Vaccine1" }
            };
            var updateUserVaccinationResponse = new UpdateUserVaccinationResponse(userVaccination.Id, userVaccination.VaccinationDate, userVaccination.Description, userVaccination.VaccineCalendarId);

            _unitOfWorkMock.Setup(u => u.UserVaccinations.GetUserVaccinationByIdAsync(userVaccination.Id)).ReturnsAsync(userVaccination);
            _mapperMock.Setup(m => m.Map(updateUserVaccinationRequest, userVaccination));
            _unitOfWorkMock.Setup(u => u.UserVaccinations.UpdateVaccinationAsync(userVaccination)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UpdateUserVaccinationResponse>(userVaccination)).Returns(updateUserVaccinationResponse);

            // Act
            var result = await _userVaccinationService.UpdateVaccinationAsync(userVaccination.Id, updateUserVaccinationRequest);

            // Assert
            Assert.That(result, Is.EqualTo(updateUserVaccinationResponse));
        }

        [Test]
        public void UpdateVaccinationAsync_InvalidRequest_ThrowsNotFoundException()
        {
            // Arrange
            var updateUserVaccinationRequest = new UpdateUserVaccinationRequest(DateOnly.FromDateTime(DateTime.Now), "Description");

            _unitOfWorkMock.Setup(u => u.UserVaccinations.GetUserVaccinationByIdAsync(It.IsAny<Guid>())).ReturnsAsync((UserVaccination?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userVaccinationService.UpdateVaccinationAsync(Guid.NewGuid(), updateUserVaccinationRequest));
        }
    }
}