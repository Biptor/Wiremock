using Infrastructure.Interfaces;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace Infrastructure
{
    public class MockServer : IMockServer
    {
        private WireMockServer? _server;
        private readonly WireMockServerSettings _settings;

        public MockServer(WireMockServer? server, WireMockServerSettings settings)
        {
            _settings = settings;
        }

        public void LoadResponse(string endpointPath, string httpMethod, string responseData)
        {
            if (_server == null)
                throw new NullReferenceException($"WireMockServer instance is null in {nameof(MockServer)}");

            _server
                .Given(
                    Request.Create().WithPath(endpointPath).UsingMethod(httpMethod)
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody(responseData)
                );
        }

        public void Start()
        {
            _server = WireMockServer.Start(_settings);
        }

        public void Stop()
        {
            _server?.Stop();
        }
    }
}