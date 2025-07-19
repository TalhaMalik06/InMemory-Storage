using InMemory_Storage.Models;
using InMemory_Storage.Repository;
using Xunit;

namespace InMemory.Storage.Tests.Repository
{
    public class KeyValueRepositoryTests
    {
        [Fact]
        public void Set_StoresValueWithoutExpiry()
        {
            // Arrange
            var repository = new KeyValueRepository();
            var key = "key1";
            var value = "value1";

            // Act
            repository.Set(key, value);

            // Assert
            var result = repository.Get(key);
            Assert.Equal(value, result);
        }

        [Fact]
        public void Set_StoresValueWithExpiry()
        {
            // Arrange
            var repository = new KeyValueRepository();
            var key = "key1";
            var value = "value1";
            var ttl = TimeSpan.FromSeconds(1);

            // Act
            repository.Set(key, value, ttl);
            var resultBeforeExpiry = repository.Get(key);

            // Wait for the key to expire
            Thread.Sleep(1100);
            var resultAfterExpiry = repository.Get(key);

            // Assert
            Assert.Equal(value, resultBeforeExpiry);
            Assert.Equal(string.Empty, resultAfterExpiry);
        }

        [Fact]
        public void Get_ReturnsEmptyStringForNonExistentKey()
        {
            // Arrange
            var repository = new KeyValueRepository();
            var key = "nonexistent";

            // Act
            var result = repository.Get(key);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Delete_RemovesKeyFromStorage()
        {
            // Arrange
            var repository = new KeyValueRepository();
            var key = "key1";
            var value = "value1";
            repository.Set(key, value);

            // Act
            repository.Delete(key);
            var result = repository.Get(key);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ContainsKey_ReturnsTrueForExistingKey()
        {
            // Arrange
            var repository = new KeyValueRepository();
            var key = "key1";
            var value = "value1";
            repository.Set(key, value);

            // Act
            var result = repository.ContainsKey(key);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ContainsKey_ReturnsFalseForNonExistentKey()
        {
            // Arrange
            var repository = new KeyValueRepository();
            var key = "nonexistent";

            // Act
            var result = repository.ContainsKey(key);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAllData_ReturnsAllStoredData()
        {
            // Arrange
            var repository = new KeyValueRepository();
            repository.Set("key1", "value1");
            repository.Set("key2", "value2");

            // Act
            var data = repository.GetAllData();

            // Assert
            Assert.Equal(2, data.Count);
            Assert.Equal("value1", data["key1"].Value);
            Assert.Equal("value2", data["key2"].Value);
        }

        [Fact]
        public void RestoreData_ReplacesExistingData()
        {
            // Arrange
            var repository = new KeyValueRepository();
            repository.Set("key1", "value1");

            var newData = new Dictionary<string, ValueWithExpiry>
            {
                { "key2", new ValueWithExpiry("value2", null) }
            };

            // Act
            repository.RestoreData(newData);
            var allData = repository.GetAllData();

            // Assert
            Assert.Single(allData);
            Assert.True(allData.ContainsKey("key2"));
            Assert.False(allData.ContainsKey("key1"));
        }
    }
}