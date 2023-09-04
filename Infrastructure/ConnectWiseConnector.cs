using Core.Domain;
using RestSharp;

namespace Infrastructure
{
    public class ConnectWiseConnector : TicketService
    {
        private readonly IRestClient _restClient;
        private readonly string _endpointUrl;

        public ConnectWiseConnector(IRestClient restClient, string endpointUrl)
        {
            _restClient = restClient;
            _endpointUrl = endpointUrl;
        }

        public async Task<Ticket?> GetTicket(Integration integration)
        {
            var request = new RestRequest(_endpointUrl, Method.Get);

            var apiResponse = await _restClient.ExecuteAsync<Ticket>(request, CancellationToken.None);

            return apiResponse.Data;
        }

        public async Task<string?> SendAsync()
        {
            var request = new RestRequest(_endpointUrl, Method.Get);

            var apiResponse = await _restClient.ExecuteAsync(request, CancellationToken.None);

            var responseData = apiResponse.Content;

            return responseData;
        }
    }
}