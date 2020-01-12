using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class CustomerInformation
    {
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

        [Obsolete("Serialization", true)]
        public CustomerInformation() { }

        private CustomerInformation(string email, string firstName, string surname, string phoneNumber, string country, string zipCode, string city, string streetNumber)
        {
            Email = email;
            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Country = country;
            ZipCode = zipCode;
            City = city;
            StreetNumber = streetNumber;
        }

        public static CustomerInformation Create(string email, string firstName, string surname, string phoneNumber, string country, string zipCode, string city, string streetName)
        {
            return new CustomerInformation(
                email,
                firstName,
                surname,
                phoneNumber,
                country,
                zipCode,
                city,
                streetName);
        }
    }
}
