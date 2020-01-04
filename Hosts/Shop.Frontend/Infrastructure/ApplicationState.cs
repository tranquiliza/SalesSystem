using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Shop.Frontend.Infrastructure
{
    public class ApplicationState : IApplicationState
    {
        private readonly IJSRuntime _jSRuntime;
        private const string _storageName = "tranquiliza-webshop-state";
        private string CreateKey(string key) => $"{_storageName}-{key}";

        public ApplicationState(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task<T> GetItem<T>(string key)
        {
            var valueFromStorage = await _jSRuntime.InvokeAsync<string>(MethodNames.GetItem, CreateKey(key)).ConfigureAwait(false);

            if (string.IsNullOrEmpty(valueFromStorage))
                return default;

            return JsonSerializer.Deserialize<T>(valueFromStorage);
        }

        public async Task SetItem<T>(string key, T value)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _jSRuntime.InvokeVoidAsync(MethodNames.SetItem, CreateKey(key), serializedValue).ConfigureAwait(false);
        }

        private static class MethodNames
        {
            public const string GetItem = "TranquilizaGetItem";
            public const string SetItem = "TranquilizaSetItem";
        }
    }
}
