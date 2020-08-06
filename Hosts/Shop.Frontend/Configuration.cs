using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
#if DEBUG
            ApiBaseAddress = "https://localhost:44311/";
#else
            ApiBaseAddress = "https://tranquiliza.dynu.net/ShopApi/";
#endif
        }

        public string ApiBaseAddress { get; }
    }
}
