using InMemory_Storage.Repository;
using Xunit;

namespace InMemory.Storage.Tests.Repository
{
    public class ListRepositoryTests
    {
        [Fact]
        public void LPush_AddsValuesToTheBeginningOfList()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";

            // Act
            repository.LPush(key, "value1", "value2");

            // Assert
            var result = repository.LRange(key, 0, -1);
            Assert.Equal(new[] { "value1", "value2" }, result);
        }

        [Fact]
        public void RPush_AddsValuesToTheEndOfList()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";

            // Act
            repository.RPush(key, "value1", "value2");

            // Assert
            var result = repository.LRange(key, 0, -1);
            Assert.Equal(new[] { "value1", "value2" }, result);
        }

        [Fact]
        public void LPop_RemovesAndReturnsFirstElement()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";
            repository.RPush(key, "value1", "value2");

            // Act
            var poppedValue = repository.LPop(key);

            // Assert
            Assert.Equal("value1", poppedValue);
            var result = repository.LRange(key, 0, -1);
            Assert.Equal(new[] { "value2" }, result);
        }

        [Fact]
        public void RPop_RemovesAndReturnsLastElement()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";
            repository.RPush(key, "value1", "value2");

            // Act
            var poppedValue = repository.RPop(key);

            // Assert
            Assert.Equal("value2", poppedValue);
            var result = repository.LRange(key, 0, -1);
            Assert.Equal(new[] { "value1" }, result);
        }

        [Fact]
        public void LPop_EmptyList_ReturnsEmptyString()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";

            // Act
            var poppedValue = repository.LPop(key);

            // Assert
            Assert.Equal(string.Empty, poppedValue);
        }

        [Fact]
        public void RPop_EmptyList_ReturnsEmptyString()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";

            // Act
            var poppedValue = repository.RPop(key);

            // Assert
            Assert.Equal(string.Empty, poppedValue);
        }

        [Fact]
        public void LRange_ReturnsCorrectRange()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";
            repository.RPush(key, "value1", "value2", "value3");

            // Act
            var result = repository.LRange(key, 1, 2);

            // Assert
            Assert.Equal(new[] { "value2", "value3" }, result);
        }

        [Fact]
        public void LRange_OutOfBounds_ReturnsEmptyArray()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";

            // Act
            var result = repository.LRange(key, 10, 20);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void LLen_ReturnsCorrectLength()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";
            repository.RPush(key, "value1", "value2");

            // Act
            var length = repository.LLen(key);

            // Assert
            Assert.Equal(2, length);
        }

        [Fact]
        public void LLen_EmptyList_ReturnsZero()
        {
            // Arrange
            var repository = new ListRepository();
            var key = "list1";

            // Act
            var length = repository.LLen(key);

            // Assert
            Assert.Equal(0, length);
        }

        [Fact]
        public void GetAllData_ReturnsAllLists()
        {
            // Arrange
            var repository = new ListRepository();
            repository.RPush("list1", "value1");
            repository.RPush("list2", "value2");

            // Act
            var data = repository.GetAllData();

            // Assert
            Assert.Equal(2, data.Count);
            Assert.Equal(new[] { "value1" }, data["list1"]);
            Assert.Equal(new[] { "value2" }, data["list2"]);
        }

        [Fact]
        public void RestoreData_ReplacesExistingData()
        {
            // Arrange
            var repository = new ListRepository();
            repository.RPush("list1", "value1");

            var newData = new Dictionary<string, List<string>>
            {
                { "list2", new List<string> { "value2" } }
            };

            // Act
            repository.RestoreData(newData);
            var allData = repository.GetAllData();

            // Assert
            Assert.Single(allData);
            Assert.True(allData.ContainsKey("list2"));
            Assert.False(allData.ContainsKey("list1"));
        }
    }
}
