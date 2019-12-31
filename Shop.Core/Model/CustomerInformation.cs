using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class CustomerInformation : DomainEntityBase
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public Guid UserId { get; private set; }

        [JsonProperty]
        public string Email { get; private set; }

        [JsonProperty]
        public string FirstName { get; private set; }

        [JsonProperty]
        public string Surname { get; private set; }

        [JsonProperty]
        public string PhoneNumber { get; private set; }

        [JsonProperty]
        public string Country { get; private set; }

        [JsonProperty]
        public string ZipCode { get; private set; }

        [JsonProperty]
        public string City { get; private set; }

        [JsonProperty]
        public string StreetNumber { get; private set; }

        [JsonProperty]
        public Guid CreatedByClientId { get; private set; }

        [Obsolete("Serialization", true)]
        public CustomerInformation() { }

        private CustomerInformation(string email, string firstName, string surname, string phoneNumber, string country, string zipCode, string city, string streetNumber, Guid clientId, Guid userId)
        {
            Id = Guid.NewGuid();
            Email = email;
            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            UserId = userId;
            Country = country;
            ZipCode = zipCode;
            City = city;
            StreetNumber = streetNumber;
            CreatedByClientId = clientId;
        }

        public static CustomerInformation Create(string email, string firstName, string surname, string phoneNumber, string country, string zipCode, string city, string streetName, Guid clientId, Guid userId = default)
        {
            return new CustomerInformation(
                email,
                firstName,
                surname,
                phoneNumber,
                country,
                zipCode,
                city,
                streetName,
                clientId,
                userId);
        }

        public bool TryUpdate(string email, string firstName, string surname, string phoneNumber, string country, string zipCode, string city, string streetNumber, IApplicationContext context)
        {
            if (string.Equals(email, Email, StringComparison.OrdinalIgnoreCase)
                && string.Equals(FirstName, firstName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Surname, surname, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Country, country, StringComparison.OrdinalIgnoreCase)
                && string.Equals(ZipCode, zipCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(City, city, StringComparison.OrdinalIgnoreCase)
                && string.Equals(StreetNumber, streetNumber, StringComparison.OrdinalIgnoreCase)
                && string.Equals(PhoneNumber, phoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                if (UserId == default && !context.IsAnonymous)
                {
                    UserId = context.UserId;
                    return true;
                }

                return false;
            }

            if (context.IsAnonymous && CreatedByClientId == context.ClientId)
            {
                Email = email;
                FirstName = firstName;
                Surname = surname;
                PhoneNumber = phoneNumber;
                Country = country;
                ZipCode = zipCode;
                City = city;
                StreetNumber = streetNumber;

                return true;
            }

            if (UserId == default)
                UserId = context.UserId;

            Email = email;
            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Country = country;
            ZipCode = zipCode;
            City = city;
            StreetNumber = streetNumber;

            return true;
        }
    }
}
