using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend
{
    public interface IConfiguration
    {
        string ApiBaseAddress { get; }
    }
}
