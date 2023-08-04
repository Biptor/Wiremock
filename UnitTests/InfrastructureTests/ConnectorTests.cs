using Infrastructure;
using Infrastructure.Interfaces;
using RestSharp;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace InfrastructureTests
{
    public class ConnectorTests
    {
        private const int API_PORT = 62737;
        private const string PRODUCT_API_URL = "http://localhost:62737/products";
        private readonly IRestClient _restClient;
        private readonly WireMockServer _server;
        private readonly IConnector _test;

        public ConnectorTests()
        {
            _restClient = new RestClient();
            _test = new Connector(_restClient);
            _server = WireMockServer.Start(API_PORT, false);
        }

        [Fact]
        public async void SendAsync_Should_Return_Not_Empty_Result()
        {
            // Arrange
            var responseData = $"{DateTime.Now:yyyy-MM-dd} Time: {DateTime.Now:HH:mm:ss}";
            _server
                .Given(
                    Request.Create().WithPath("/products").UsingMethod("GET")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody(responseData)
                );

            // Act
            var result = await _test.SendAsync(PRODUCT_API_URL, RestSharp.Method.Get);

            // Assert
            Assert.Equal(responseData, result);
        }
    }
}