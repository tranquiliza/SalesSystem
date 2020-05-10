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
            ApiBaseAddress = "https://localhost:44311/";
            //ApiBaseAddress = "https://tranquiliza.dynu.net/ShopApi/";
        }

        public string ApiBaseAddress { get; }
    }
}
