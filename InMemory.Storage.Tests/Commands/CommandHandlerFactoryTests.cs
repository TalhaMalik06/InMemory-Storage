using InMemory_Storage.Commands;
using InMemory_Storage.Messages;
using Moq;
using Xunit;

namespace InMemory.Storage.Tests.Commands
{
    public class CommandHandlerFactoryTests
    {
        [Fact]
        public void Constructor_NullHandlers_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CommandHandlerFactory(null!));
        }

        [Fact]
        public void GetCommandHandler_NullCommandName_ThrowsArgumentException()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler>();
            var factory = new CommandHandlerFactory(new[] { mockHandler.Object });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => factory.GetCommandHandler(null!));
            Assert.Equal(ErrorMessages.CommandNameCannotBeNull, exception.Message);
        }

        [Fact]
        public void GetCommandHandler_EmptyCommandName_ThrowsArgumentException()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler>();
            var factory = new CommandHandlerFactory(new[] { mockHandler.Object });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => factory.GetCommandHandler(string.Empty));
            Assert.Equal(ErrorMessages.CommandNameCannotBeNull, exception.Message);
        }

        [Fact]
        public void GetCommandHandler_WhitespaceCommandName_ThrowsArgumentException()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler>();
            var factory = new CommandHandlerFactory(new[] { mockHandler.Object });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => factory.GetCommandHandler("   "));
            Assert.Equal(ErrorMessages.CommandNameCannotBeNull, exception.Message);
        }

        [Fact]
        public void GetCommandHandler_NoMatchingHandler_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler>();
            mockHandler.Setup(h => h.CanHandle(It.IsAny<string>())).Returns(false);
            var factory = new CommandHandlerFactory(new[] { mockHandler.Object });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => factory.GetCommandHandler("UNKNOWN"));
            Assert.Equal(string.Format(ErrorMessages.NoCommandHandlerFound, "UNKNOWN"), exception.Message);
        }

        [Fact]
        public void GetCommandHandler_MatchingHandler_ReturnsHandler()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler>();
            mockHandler.Setup(h => h.CanHandle("MATCH")).Returns(true);
            var factory = new CommandHandlerFactory(new[] { mockHandler.Object });

            // Act
            var handler = factory.GetCommandHandler("MATCH");

            // Assert
            Assert.Equal(mockHandler.Object, handler);
        }
    }
}
