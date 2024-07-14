using InMemory_Storage.Messages;
using InMemory_Storage.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public class KeyValueRepository : IKeyValueRepository
    {
        public KeyValueRepository(IKeyValueStorage keyValueStorage)
        {
            KeyValueStorage = keyValueStorage ?? throw new ArgumentNullException(nameof(keyValueStorage));
        }

        private IKeyValueStorage KeyValueStorage { get; }

        public void DeleteItem(string key)
        {
            throw new NotImplementedException();
        }

        public string GetItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(string.Format(ErrorMessages.FieldCannotBeNullOrEmpty, nameof(key)), nameof(key));
            }

            return KeyValueStorage.Get(key) ?? "NULL";

        }

        public void SetItem(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(string.Format(ErrorMessages.FieldCannotBeNullOrEmpty, nameof(key)), nameof(key));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(string.Format(ErrorMessages.FieldCannotBeNullOrEmpty, nameof(value)), nameof(value));
            }

            KeyValueStorage.Set(key, value);
        }
    }
}
