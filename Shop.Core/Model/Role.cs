using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class Role
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        [Obsolete("Serialization", true)]
        public Role() { }

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
