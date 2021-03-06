﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Shop.Frontend.Infrastructure
{
    public static class Serialization
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string Serialize<T>(T input)
        {
            return JsonSerializer.Serialize(input, _options);
        }

        public static T Deserialize<T>(string input)
        {
            return JsonSerializer.Deserialize<T>(input, _options);
        }
    }
}
