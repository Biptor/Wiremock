using Infrastructure.Interfaces;
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