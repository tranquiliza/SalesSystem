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

        public bool TryUpdate(string email, string firstName, string surname, string address, string phoneNumber, IApplicationContext context)
        {
            if (string.Equals(email, Email, StringComparison.OrdinalIgnoreCase)
                && string.Equals(FirstName, firstName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Surname, surname, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Address, address, StringComparison.OrdinalIgnoreCase)
                && string.Equals(PhoneNumber, phoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                if (UserId == default && !context.IsAnonymous)
                {
                    UserId = context.UserId;
                    return true;
                }

                return false;
            }

            if (context.IsAnonymous)
            {
                FirstName = firstName;
                Surname = surname;
                Address = address;
                PhoneNumber = phoneNumber;

                return true;
            }

            if (UserId == default)
                UserId = context.UserId;

            Email = email;
            FirstName = firstName;
            Surname = surname;
            Address = address;
            PhoneNumber = phoneNumber;

            return true;
        }
    }
}
