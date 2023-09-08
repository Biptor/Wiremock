using Infrastructure;
using RestSharp;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;


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

        private IngestionService _ingestionService;

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

            TicketService connectWiseConnector = new ConnectWiseConnector(_restClient, TICKETS_API_URL);
            Discovery watsonConnector = new WatsonConnector(_restClient, DISCOVERY_API_URL);
            _ingestionService = new IngestionService(connectWiseConnector, watsonConnector);

            _connectWiseServer = WireMockServer.Start(CONNECTWISE_API_PORT, false);
            _watsonServer = WireMockServer.Start(WATSON_API_PORT, false);
        }

        [TearDown]
        public void TearDown()
        {
            _connectWiseServer.Stop();
            _watsonServer.Stop();
        }

        [Test]
        public async Task SendAsync_Should_Receive_Discovery_Record()
        {
            // Arrange
            // Given: A simple ticket comming from the Tickets API of ConnectWise
            _connectWiseServer
                .Given(
                    Request.Create().WithPath(TICKETS_ENDPOINT_PATH).UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(
                            new { id = "MyId", name = "My Name", description = "This is my description" })
                        )
                );

            // And: Watson Discovery is able to receive the ticket
            _watsonServer
                .Given(
                    Request.Create().WithPath(DISCOVERY_ENDPOINT_PATH).UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("Content-Type", "application/json")                        
                );


            // Act
            // When: Run the ingestion service for ConnectWise with service board
            var integration = Integration;
            await _ingestionService.Ingest(integration);

            // Assert                    
            // Then: Recive the simple ticket in Watson Discovery with time entries
            var discoveryRecord = JsonConvert.SerializeObject(new DiscoveryRecord
            {
                Id = "MyId",
                Name = "My NameAny Transformation Rule",
                Description = "This is my description"
            }, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            var watsonEntries = _watsonServer.FindLogEntries(
                Request.Create().WithPath(DISCOVERY_ENDPOINT_PATH).UsingPost().WithBody(discoveryRecord)
            );

            // https://github.com/WireMock-Net/WireMock.Net/wiki/Stubbing#verify-interactions
            // https://github.com/WireMock-Net/WireMock.Net/wiki/FluentAssertions
            Assert.IsNotEmpty(watsonEntries);
        }

        // We have this new property to hold a valid integration
        private Integration Integration
            => new Integration
            {
                Id = "4242424242424242",
                Name = "ConnectWiseMR",
                Description = "This is a ConnectWise Integration"
            };
    }
}