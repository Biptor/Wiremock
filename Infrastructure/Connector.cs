using Infrastructure.Interfaces;
using RestSharp;

namespace Infrastructure
{
    public class Connector : IConnector
    {
        private readonly IRestClient _restClient;

        public Connector(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<string?> SendAsync(string endpointUrl, Method method)
        {
            var request = new RestRequest(endpointUrl, method);
            var apiResponse = await _restClient.ExecuteAsync(request, CancellationToken.None);

            var responseData = apiResponse.Content;

            return responseData;
        }
    }
}