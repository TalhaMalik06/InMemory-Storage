using InMemory_Storage.Commands;
using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using Moq;
using Xunit;

namespace InMemory.Storage.Tests.Commands
{
    public class SetWithExpiryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidSetWithExpiryCommand_ReturnsSuccessCode()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetWithExpiryCommandHandler(mockRepository.Object);
            var command = "SETEX key 60 value";

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseMessages.SuccessCode, result);
            mockRepository.Verify(repo => repo.Set("key", "value", TimeSpan.FromSeconds(60)), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidSetWithExpiryCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetWithExpiryCommandHandler(mockRepository.Object);
            var command = "SETEX key value"; // Missing TTL

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForSetWithExpiry, exception.Message);
        }

        [Fact]
        public async Task Handle_InvalidTTL_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetWithExpiryCommandHandler(mockRepository.Object);
            var command = "SETEX key invalidTTL value"; // Invalid TTL

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForSetWithExpiry, exception.Message);
        }

        [Fact]
        public async Task Handle_EmptyCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetWithExpiryCommandHandler(mockRepository.Object);
            var command = ""; // Empty command

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForSetWithExpiry, exception.Message);
        }

        [Fact]
        public void CanHandle_ValidSetWithExpiryCommandType_ReturnsTrue()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetWithExpiryCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("SETEX");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_InvalidCommandType_ReturnsFalse()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetWithExpiryCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("GET");

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SetWithExpiryCommandHandler(null!));
        }
    }
}
