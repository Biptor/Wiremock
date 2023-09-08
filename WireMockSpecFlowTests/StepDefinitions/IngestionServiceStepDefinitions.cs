using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WireMock.Server;
using Core.Domain;
using Infrastructure;
using RestSharp;
using NUnit.Framework;

namespace WireMockSpecFlowTests.StepDefinitions
{
    [Binding]
    public class IngestionServiceStepDefinitions
    {
        // TODO: Configure randon ports
        private const int CONNECTWISE_API_PORT = 62737;
        private const string TICKETS_ENDPOINT_PATH = "/tickets";
        private const string TICKETS_API_URL = "http://localhost:62737/tickets";

        // TODO: Configure randon ports
        private const int WATSON_API_PORT = 62738;
        private const string DISCOVERY_ENDPOINT_PATH = "/discovery";
        private const string DISCOVERY_API_URL = "http://localhost:62738/discovery";

        private static WireMockServer _connectWiseServer;
        private static WireMockServer _watsonServer;

        private IngestionService _ingestionService = initIngestionService();

        static IngestionService initIngestionService()
        {
            var options = new RestClientOptions()
            {
                ThrowOnAnyError = true
            };
            IRestClient _restClient = new RestClient(options);
            TicketService connectWiseConnector = new ConnectWiseConnector(_restClient, TICKETS_API_URL);
            Discovery watsonConnector = new WatsonConnector(_restClient, DISCOVERY_API_URL);
            return new IngestionService(connectWiseConnector, watsonConnector);
        }

        [Given(@"A simple ticket comming from the Tickets API of ConnectWise")]
        public void GivenASimpleTicketCommingFromTheTicketsAPIOfConnectWise()
        {
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
        }

        [Given(@"Watson Discovery is able to receive the ticket")]
        public void GivenWatsonDiscoveryIsAbleToReceiveTheTicket()
        {
            _watsonServer
                .Given(
                    Request.Create().WithPath(DISCOVERY_ENDPOINT_PATH).UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("Content-Type", "application/json")
                );
        }

        [When(@"Run the ingestion service for ConnectWise with service board")]
        public async Task WhenRunTheIngestionServiceForConnectWiseWithServiceBoard()
        {
            var integration = Integration;
            await _ingestionService.Ingest(integration);
        }

        [Then(@"Watson Discovery ingests the simple ticket with time entries")]
        public void ThenWatsonDiscoveryIngestsTheSimpleTicketWithTimeEntries()
        {
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

        // TODO: Review this initiation pattern
        // We have this new property to hold a valid integration
        private Integration Integration
            => new Integration
            {
                Id = "4242424242424242",
                Name = "ConnectWiseMR",
                Description = "This is a ConnectWise Integration"
            };


        static WireMockServer initConnectWise()
        {
            return WireMockServer.Start(new WireMock.Settings.WireMockServerSettings
            {
                Port = CONNECTWISE_API_PORT,
                UseSSL = false
            });
        }

        static WireMockServer initWatson()
        {
            return WireMockServer.Start(new WireMock.Settings.WireMockServerSettings
            {
                Port = WATSON_API_PORT,
                UseSSL = false
            });
        }


        [BeforeFeature]
        public static void StartServers()
        {
            _connectWiseServer = initConnectWise();
            _watsonServer = initWatson();
        }

        [BeforeScenario("Ingestion")]
        public void BeforeScenario()
        {
            _connectWiseServer.Reset();
            _watsonServer.Reset();
        }

        [AfterFeature]
        public static void StopServers()
        {
            _connectWiseServer.Stop();
            _watsonServer.Stop();
        }


    }
}
