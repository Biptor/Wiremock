using Infrastructure;
using RestSharp;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Core.Domain;
using static WireMockSpecFlowTests.Domain.IntegrationTestDataBuilder;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WireMockSpecFlowTests.Domain;

namespace InfrastructureTests
{
    [TestFixture]
    public class IngestionIntegrationTests
    {
        // TODO: Configure randon ports
        private const int CONNECTWISE_API_PORT = 62737;
        private const string TICKETS_ENDPOINT_PATH = "/tickets";
        private const string TICKETS_API_URL = "http://localhost:62737/tickets";

        // TODO: Configure randon ports
        private const int WATSON_API_PORT = 62738;
        private const string DISCOVERY_ENDPOINT_PATH = "/discovery";
        private const string DISCOVERY_API_URL = "http://localhost:62738/discovery";

        private IRestClient _restClient;

        private WireMockServer _connectWiseServer;
        private WireMockServer _watsonServer;

        [SetUp]
        protected void SetUp()
        {
            var options = new RestClientOptions()
            {
                ThrowOnAnyError = true
            };
            _restClient = new RestClient(options);

            _connectWiseServer = WireMockServer.Start(CONNECTWISE_API_PORT, false);
            _watsonServer = WireMockServer.Start(WATSON_API_PORT, false);
        }

        [TearDown]
        public void TearDown()
        {
            _connectWiseServer.Stop();
            _watsonServer.Stop();
        }

    }
}