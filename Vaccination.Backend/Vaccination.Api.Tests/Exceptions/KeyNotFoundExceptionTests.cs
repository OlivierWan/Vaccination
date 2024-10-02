namespace Vaccination.Api.Tests.Exceptions
{
    [TestFixture]
    public class KeyNotFoundExceptionTests
    {
        [Test]
        public void KeyNotFoundException_ShouldSetMessage()
        {
            // Arrange
            var expectedMessage = "This key was not found";

            // Act
            var exception = new Api.Exceptions.KeyNotFoundException(expectedMessage);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void KeyNotFoundException_ShouldBeOfTypeException()
        {
            // Arrange
            var expectedMessage = "This key was not found";

            // Act
            var exception = new Api.Exceptions.KeyNotFoundException(expectedMessage);

            // Assert
            Assert.That(exception, Is.InstanceOf<Exception>());
        }
    }
}