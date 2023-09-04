using Core.Domain;
using Core.Interfaces;
using Core.UseCases;
using Moq;
using DateTime = System.DateTime;

namespace Core.UnitTests.UseCases
{
    public class GetDateTimeProductApiUseCaseTests
    {
        private readonly Mock<IConnector> _connector;
        private readonly IGetDateTimeProductApiUseCase _test;

        public GetDateTimeProductApiUseCaseTests()
        {
            _connector = new Mock<IConnector>();
            _test = new GetDateTimeProductApiUseCase(_connector.Object);
        }

        [Fact]
        public async void ExecuteAsync_Should_Return_Not_Empty_Result()
        {
            // Arrange
            var responseData = $"{DateTime.Now:yyyy-MM-dd} Time: {DateTime.Now:HH:mm:ss}";
            _connector.Setup(c => c.SendAsync(It.IsAny<string>())).ReturnsAsync(responseData);

            // Act
            var result = await _test.ExecuteAsync("param");

            // Assert
            Assert.Equal(responseData, result);
        }
    }
}