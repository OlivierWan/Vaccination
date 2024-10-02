using Vaccination.Api.Exceptions;

namespace Vaccination.Api.Tests.Exceptions
{
    [TestFixture]
    public class DatabaseExceptionTests
    {
        [Test]
        public void DatabaseException_ShouldSetMessage()
        {
            // Arrange
            var expectedMessage = "This is a database exception";

            // Act
            var exception = new DatabaseException(expectedMessage);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void DatabaseException_ShouldBeOfTypeException()
        {
            // Arrange
            var expectedMessage = "This is a database exception";

            // Act
            var exception = new DatabaseException(expectedMessage);

            // Assert
            Assert.That(exception, Is.InstanceOf<Exception>());
        }
    }
}
