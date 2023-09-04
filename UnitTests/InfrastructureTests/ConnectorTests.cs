using Infrastructure;
using RestSharp;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfrastructureTests
{
    public class ConnectorTests : IDisposable
    {
        private const int CONNECTWISE_API_PORT = 62737;
        private const string TICKETS_ENDPOINT_PATH = "/tickets";
        private const string TICKETS_API_URL = "http://localhost:62737/tickets";

        private const int WATSON_API_PORT = 62738;
        private const string DISCOVERY_ENDPOINT_PATH = "/discovery";
        private const string DISCOVERY_API_URL = "http://localhost:62738/discovery";

        private readonly IRestClient _restClient;

        private readonly IngestionService _ingestionService;

        private readonly WireMockServer _connectWiseServer;
        private readonly WireMockServer _watsonServer;

        public ConnectorTests()
        {
            var options = new RestClientOptions()
            {
                ThrowOnAnyError = true
            };
            _restClient = new RestClient(options);

            TicketService connectWiseConnector = new ConnectWiseConnector(_restClient, TICKETS_API_URL);
            Discovery watsonConnector = new WatsonConnector(_restClient, DISCOVERY_API_URL);
            _ingestionService = new IngestionService(connectWiseConnector, watsonConnector);

            _connectWiseServer = WireMockServer.Start(CONNECTWISE_API_PORT, false);
            _watsonServer = WireMockServer.Start(WATSON_API_PORT, false);
        }

        public void Dispose()
        {
            _connectWiseServer.Stop();
            _watsonServer.Stop();
        }

        [Fact]
        public async void SendAsync_Should_Receive_Discovery_Record()
        {
            // Arrange            
            _connectWiseServer
                .Given(
                    Request.Create().WithPath(TICKETS_ENDPOINT_PATH).UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(Newtonsoft.Json.JsonConvert.SerializeObject(
                            new { Id = "MyId", Name = "My Name", Description = "This is my description" })
                        )
                );

            _watsonServer
                .Given(
                    Request.Create().WithPath(DISCOVERY_ENDPOINT_PATH).UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("Content-Type", "application/json")                        
                );

            var integration = Integration;

            var discoveryRecord = JsonConvert.SerializeObject(new DiscoveryRecord
            {
                Id = "MyId",
                Name = "My NameAny Transformation Rule",
                Description = "This is my description"
            }, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            // Act
            await _ingestionService.Ingest(integration);

            // Assert                    
            var watsonEntries = _watsonServer.FindLogEntries(
                Request.Create().WithPath(DISCOVERY_ENDPOINT_PATH).UsingPost().WithBody(discoveryRecord)
            );

            // https://github.com/WireMock-Net/WireMock.Net/wiki/Stubbing#verify-interactions
            // https://github.com/WireMock-Net/WireMock.Net/wiki/FluentAssertions
            Assert.NotEmpty(watsonEntries);
        }

        // We have this new property to hold a valid integration
        private Integration Integration
            //                 ^^^^^
            => new Integration
            {
                Id = "4242424242424242",
                Name = "ConnectWiseMR",
                Description = "This is a ConnectWise Integration"
            };
    }
}