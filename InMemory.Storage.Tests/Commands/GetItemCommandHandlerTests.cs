using InMemory_Storage.Commands;
using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory.Storage.Tests.Commands
{
    public class GetItemCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidGetCommand_ReturnsValue()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            mockRepository.Setup(r => r.Get("key")).Returns("value");
            var handler = new GetItemCommandHandler(mockRepository.Object);
            var command = "GET key";

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("value", result);
            mockRepository.Verify(repo => repo.Get("key"), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidGetCommand_ReturnsEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            mockRepository.Setup(r => r.Get("key")).Returns((string?)null!);
            var handler = new GetItemCommandHandler(mockRepository.Object);
            var command = "GET key";

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            mockRepository.Verify(repo => repo.Get("key"), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidGETCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new GetItemCommandHandler(mockRepository.Object);
            var command = "GET"; // Missing value

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForGet, exception.Message);
        }

        [Fact]
        public async Task Handle_EmptyCommand_ThrowsCommandFormatException()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new GetItemCommandHandler(mockRepository.Object);
            var command = ""; // Empty command

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CommandFormatException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(ErrorMessages.InvalidCommandFormatForGet, exception.Message);
        }

        [Fact]
        public void CanHandle_ValidGetCommandType_ReturnsTrue()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new GetItemCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("GET");

            // Assert
            Assert.True(canHandle);
        }

        [Fact]
        public void CanHandle_InvalidCommandType_ReturnsFalse()
        {
            // Arrange
            var mockRepository = new Mock<IKeyValueRepository>();
            var handler = new GetItemCommandHandler(mockRepository.Object);

            // Act
            var canHandle = handler.CanHandle("SET");

            // Assert
            Assert.False(canHandle);
        }

        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GetItemCommandHandler(null!));
        }
    }

}
