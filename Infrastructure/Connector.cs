using Core.Domain;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System.Net;

namespace Infrastructure
{
    public class Connector : IConnector
    {
        private readonly IRestClient _restClient;
        private readonly MockServer mockServer;

        public Connector(IRestClient restClient)
        {
            _restClient = restClient;
            mockServer = MockServer.GetInstance();
        }

        public async Task<string?> SendAsync(string endpointUrl)
        {
            var request = new RestRequest(endpointUrl, Method.Get);
            var apiResponse = await _restClient.ExecuteAsync(request, CancellationToken.None);

            var responseData = apiResponse.Content;

            return responseData;
        }

        public async Task<bool> SendFileAsync(string endpointUrl, IFormFile file, string rutaFullCompleta)
        {
            mockServer.LoadFileResponse(endpointUrl, file);

            var request = new RestRequest(endpointUrl, Method.Post);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile(file.FileName, rutaFullCompleta);

            RestResponse response = await _restClient.ExecuteAsync(request, CancellationToken.None);

            return response.StatusCode != 0 && response.StatusCode == HttpStatusCode.OK;
        }
    }
}