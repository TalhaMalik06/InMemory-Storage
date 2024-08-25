namespace InMemory_Storage.Models
{
    public class ValueWithExpiry
    {
        public ValueWithExpiry(string value, DateTime? expiry) 
        {
            Value = value;
            Expiry = expiry;
        }
        public string Value { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
