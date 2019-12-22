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
        public string Address { get; private set; }

        [JsonProperty]
        public string PhoneNumber { get; private set; }

        [Obsolete("Serialization", true)]
        public CustomerInformation() { }

        private CustomerInformation(string email, string firstName, string surname, string address, string phoneNumber, Guid userId)
        {
            Id = Guid.NewGuid();
            Email = email;
            FirstName = firstName;
            Surname = surname;
            Address = address;
            PhoneNumber = phoneNumber;
            UserId = userId;
        }

        public static CustomerInformation Create(string email, string firstName, string surname, string address, string phoneNumber, Guid userId = default)
        {
            return new CustomerInformation(email, firstName, surname, address, phoneNumber, userId);
        }
    }
}
