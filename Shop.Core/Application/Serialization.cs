﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public static class Serialization
    {
        public static string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public static T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
    }
}
