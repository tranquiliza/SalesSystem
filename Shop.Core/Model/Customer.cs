using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string Surname { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }

        [Obsolete("Serialization", true)]
        public Customer() { }

        private Customer(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
        }

        public static Customer Create(string email)
        {
            return new Customer(email);
        }
    }
}
