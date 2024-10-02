using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Application.Dtos.Calendar;
using Vaccination.Application.Mapper;
using Vaccination.Domain.Entities;

namespace Vaccination.Application.Tests.Mapper
{
    [TestFixture]
    public class CalendarProfileTests
    {
        private IMapper mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CalendarProfile>();
            });
            mapper = config.CreateMapper();
        }

      

        [Test]
        public void Should_Map_CalendarVaccination_To_CalendarVaccinationResponse()
        {
            // Arrange
            var calendarVaccination = new CalendarVaccination
            {
                Id = Guid.NewGuid(),
                Name = "Test Vaccine",
                Description = "Test Description",
                MonthAge = 12,
                MonthDelay = 2,
                CreatedBy = Guid.NewGuid(),
                CreatedOnUtc = DateTime.UtcNow,
                IsDeleted = false
            };

            // Act
            var result = mapper.Map<CalendarVaccinationResponse>(calendarVaccination);

            // Assert
            Assert.That(result.Id, Is.EqualTo(calendarVaccination.Id));
            Assert.That(result.Name, Is.EqualTo(calendarVaccination.Name));
            Assert.That(result.Description, Is.EqualTo(calendarVaccination.Description));
            Assert.That(result.MonthAge, Is.EqualTo(calendarVaccination.MonthAge));
            Assert.That(result.MonthDelay, Is.EqualTo(calendarVaccination.MonthDelay));
        }

        

        [Test]
        public void Should_Map_GetCalendarVaccinationByIdRequest_To_CalendarVaccination()
        {
            // Arrange
            var request = new GetCalendarVaccinationByIdRequest(Guid.NewGuid());

            // Act
            var result = mapper.Map<CalendarVaccination>(request);

            // Assert
            Assert.That(result.Id, Is.EqualTo(request.Id));
        }
    }
}