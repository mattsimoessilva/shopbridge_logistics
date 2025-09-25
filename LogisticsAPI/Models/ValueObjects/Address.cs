namespace LogisticsAPI.Models.ValueObjects
{
    public class Address : IEquatable<Address>
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }
        public string Country { get; }

        public Address(string street, string city, string state, string postalCode, string country)
        {
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public override bool Equals(object? obj) => Equals(obj as Address);

        public bool Equals(Address? other) =>
            other != null &&
            Street == other.Street &&
            City == other.City &&
            State == other.State &&
            PostalCode == other.PostalCode &&
            Country == other.Country;

        public override int GetHashCode() =>
            HashCode.Combine(Street, City, State, PostalCode, Country);
    }
}