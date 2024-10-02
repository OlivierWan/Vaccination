using Vaccination.Api.Exceptions;

namespace Vaccination.Api.Tests.Exceptions
{
    [TestFixture]
    public class SecurityTokenExceptionTests
    {
        [Test]
        public void SecurityTokenException_ShouldSetMessage()
        {
            // Arrange
            var expectedMessage = "This is a security token exception";

            // Act
            var exception = new SecurityTokenException(expectedMessage);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void SecurityTokenException_ShouldBeOfTypeException()
        {
            // Arrange
            var expectedMessage = "This is a security token exception";

            // Act
            var exception = new SecurityTokenException(expectedMessage);

            // Assert
            Assert.That(exception, Is.InstanceOf<Exception>());
        }
    }
}