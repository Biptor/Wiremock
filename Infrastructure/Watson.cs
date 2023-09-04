using RestSharp;
using Core.Domain;

namespace Infrastructure
{
    public class WatsonConnector : Discovery
    {
        private readonly IRestClient _restClient;
        private readonly string _endpointUrl;

        public WatsonConnector(IRestClient restClient, string endpointUrl)
        {
            _restClient = restClient;
            _endpointUrl = endpointUrl;
        }

        public async Task Load(DiscoveryRecord record, Integration integration)
        {
            var request = new RestRequest(_endpointUrl, Method.Post);
            request.AddJsonBody(record, ContentType.Json);
            await _restClient.PostAsync<DiscoveryRecord>(request, CancellationToken.None);
        }

    }
}
