using InMemory_Storage.Commands;
using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using Moq;
using Xunit;

namespace InMemory.Storage.Tests.Commands
{
    public class DeleteItemCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidDeleteCommand_ReturnsSuccessCode()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new DeleteItemCommandHandler(mockRepository.Object);
            var command = "DELETE key";

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseMessages.SuccessCode, result);
            mockRepository.Verify(repo => repo.Delete("key"), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidDeleteCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new DeleteItemCommandHandler(mockRepository.Object);
            var command = "DELETE";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForDelete, exception.Message);
        }

        [Fact]
        public async Task Handle_EmptyCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new DeleteItemCommandHandler(mockRepository.Object);
            var command = "";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForDelete, exception.Message);
        }

        [Fact]
        public void CanHandle_ValidDeleteCommandType_ReturnsTrue()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new DeleteItemCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("DELETE");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_InvalidCommandType_ReturnsFalse()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new DeleteItemCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("SET");

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DeleteItemCommandHandler(null!));
        }
    }
}
