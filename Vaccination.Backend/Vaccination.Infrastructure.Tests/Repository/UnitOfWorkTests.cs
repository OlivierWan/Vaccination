using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Exceptions;
using Vaccination.Infrastructure.Repositories;

namespace Vaccination.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private Mock<VaccinationContext> mockContext;
        private UnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            mockContext = new Mock<VaccinationContext>(new DbContextOptions<VaccinationContext>());
            unitOfWork = new UnitOfWork(mockContext.Object);
        }

        [TearDown]
        public void TearDown()
        {
            unitOfWork.Dispose();
        }

        [Test]
        public async Task SaveAsync_Should_Call_SaveChangesAsync_Once()
        {
            // Act
            await unitOfWork.SaveAsync();

            // Assert
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void SaveAsync_Should_Throw_DatabaseException_On_Exception()
        {
            // Arrange
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act & Assert
            var ex = Assert.ThrowsAsync<DatabaseException>(() => unitOfWork.SaveAsync());
            Assert.That(ex.Message, Is.EqualTo("Une erreur est survenue de la sauvegarde des changements dans la base de données"));
        }
    }
}