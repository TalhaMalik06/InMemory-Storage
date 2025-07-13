using InMemory_Storage.Commands;
using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using Moq;
using Xunit;

namespace InMemory.Storage.Tests.Commands
{
    public class SetItemCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidSetCommand_ReturnsSuccessCode()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetItemCommandHandler(mockRepository.Object);
            var command = "SET key value";

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseMessages.SuccessCode, result);
            mockRepository.Verify(repo => repo.Set("key", "value", null), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidSetCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetItemCommandHandler(mockRepository.Object);
            var command = "SET key"; // Missing value

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForSet, exception.Message);
        }

        [Fact]
        public async Task Handle_EmptyCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetItemCommandHandler(mockRepository.Object);
            var command = ""; // Empty command

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForSet, exception.Message);
        }

        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SetItemCommandHandler(null!));
        }

        [Fact]
        public void CanHandle_ValidSetCommandType_ReturnsTrue()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetItemCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("SET");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_InvalidCommandType_ReturnsFalse()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new SetItemCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("GET");

            // Assert
            Assert.False(canHandle);
        }
    }
}
