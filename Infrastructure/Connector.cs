﻿using Core.Domain;
using RestSharp;

namespace Infrastructure
{
    public class Connector : IConnector
    {
        private readonly IRestClient _restClient;
        private readonly string _endpointUrl;

        public Connector(IRestClient restClient, string endpointUrl)
        {
            _restClient = restClient;
            _endpointUrl = endpointUrl;
        }

        public async Task<string?> SendAsync(string param)
        {
            var request = new RestRequest(_endpointUrl, Method.Get);
            var apiResponse = await _restClient.ExecuteAsync(request, CancellationToken.None);

            var responseData = apiResponse.Content;

            return responseData;
        }
    }
}