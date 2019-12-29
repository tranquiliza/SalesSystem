using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public class QueryParam
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public QueryParam(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ApiGateway : IApiGateway
    {
        private readonly string _apiBaseAddress;
        private readonly HttpClient _httpClient;
        private readonly IApplicationStateManager _applicationStateManager;

        public ApiGateway(IConfiguration configuration, IApplicationStateManager applicationStateManager, HttpClient httpClient)
        {
            _apiBaseAddress = configuration.ApiBaseAddress;
            _httpClient = httpClient;
            _applicationStateManager = applicationStateManager;
        }

        public async Task<T> Get<T>(string controller, string action = null, string routeValue = null, params QueryParam[] queryParams)
        {
            var requestUri = BuildRequestUri(controller, action, routeValue, queryParams);
            var request = await BuildBaseRequest("GET", requestUri).ConfigureAwait(false);

            request.RequestUri = requestUri;

            Console.WriteLine("Making request");
            return await ExecuteRequest<T>(request).ConfigureAwait(false);
        }

        public async Task<T> Post<T, Y>(Y model, string controller, string action = null, string routeValue = null, params QueryParam[] queryParams)
        {
            var requestUri = BuildRequestUri(controller, action, routeValue, queryParams);

            var request = await BuildBaseRequest("POST", requestUri).ConfigureAwait(false);
            request.RequestUri = requestUri;
            request.Content = new StringContent(Serialization.Serialize(model));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await ExecuteRequest<T>(request).ConfigureAwait(false);
        }

        private async Task<T> ExecuteRequest<T>(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                // FUCK
            }

            var responseValue = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Serialization.Deserialize<T>(responseValue);
        }

        private async Task<HttpRequestMessage> BuildBaseRequest(string requestType, Uri requestUri)
        {
            var jwtToken = await _applicationStateManager.GetJwtToken().ConfigureAwait(false);
            var clientId = await _applicationStateManager.GetClientId().ConfigureAwait(false);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(requestType),
                RequestUri = requestUri
            };

            requestMessage.Headers.TryAddWithoutValidation("clientId", clientId.ToString());

            return requestMessage;
        }

        private Uri BuildRequestUri(string controller, string action = null, string routeValue = null, params QueryParam[] queryParams)
        {
            var routeBuilder = new StringBuilder();
            routeBuilder.Append(_apiBaseAddress);
            if (!_apiBaseAddress.EndsWith("/"))
                routeBuilder.Append("/");

            routeBuilder.Append(controller);

            if (!string.IsNullOrEmpty(action))
                routeBuilder.Append("/").Append(action);

            if (!string.IsNullOrEmpty(routeValue))
                routeBuilder.Append("/").Append(routeValue);

            for (int i = 0; i < queryParams.Length; i++)
            {
                if (i == 0)
                    routeBuilder.Append("?");
                else
                    routeBuilder.Append("&");

                routeBuilder.Append(queryParams[i].Name).Append("=").Append(queryParams[i].Value);
            }

            return new Uri(routeBuilder.ToString());
        }
    }
}
