namespace Vaccination.Api.Tests.Exceptions
{
    [TestFixture]
    public class UnauthorizedAccessExceptionTests
    {
        [Test]
        public void UnauthorizedAccessException_ShouldSetMessage()
        {
            // Arrange
            var expectedMessage = "This is an unauthorized access exception";

            // Act
            var exception = new Api.Exceptions.UnauthorizedAccessException(expectedMessage);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void UnauthorizedAccessException_ShouldBeOfTypeException()
        {
            // Arrange
            var expectedMessage = "This is an unauthorized access exception";

            // Act
            var exception = new Api.Exceptions.UnauthorizedAccessException(expectedMessage);

            // Assert
            Assert.That(exception, Is.InstanceOf<Exception>());
        }
    }
}