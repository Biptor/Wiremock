using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using WireMock.Matchers;
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
        private static MockServer instance;
        private static object locker = new object();
        private const int MOCK_PORT = 3001;

        protected MockServer()
        {
            if (_server is null)
            {
                _server = WireMockServer.Start(MOCK_PORT, false);
            }
        }

        public static MockServer GetInstance()
        {
            if (instance is null)
            {
                lock (locker)
                {
                    if (instance is null)
                    {
                        instance = new MockServer();
                    }
                }
            }
            return instance;
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

        public void LoadFileResponse(string endpointUrl, IFormFile file)
        {
            var textPlainContentMatcher = new ExactMatcher("test");
            var textPlainMatcher = new MimePartMatcher(MatchBehaviour.AcceptOnMatch, null, null, null, textPlainContentMatcher);
            var matchers = new IMatcher[]
            {
                textPlainMatcher
            };

            if (_server == null)
                throw new NullReferenceException($"WireMockServer instance is null in {nameof(MockServer)}");

            _server
                .Given(
                    Request.Create().WithPath($"/uploadfile")
                    .WithMultiPart(matchers)
                    .UsingMethod("POST")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
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