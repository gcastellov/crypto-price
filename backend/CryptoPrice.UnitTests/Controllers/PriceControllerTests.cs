using System.Threading.Tasks;
using CryptoPrice.Controllers;
using CryptoPrice.CryptoProviders;
using CryptoPrice.Dtos;
using CryptoPrice.Hubs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CryptoPrice.UnitTests.Controllers
{
    public class PriceControllerTests
    {
        private const string CryptoCurrency = "XRP";
        private const string Currency = "USD";

        private readonly PriceController _controller;
        private readonly Mock<ICryptoProvider> _cryptoProviderMock;
        private readonly Mock<IPriceTaskPool> _priceHubPoolMock;
        
        public PriceControllerTests()
        {
            _cryptoProviderMock = new Mock<ICryptoProvider>();
            _priceHubPoolMock = new Mock<IPriceTaskPool>();
            _controller = new PriceController(_cryptoProviderMock.Object, _priceHubPoolMock.Object);
        }

        [Fact]
        public async Task GivenBackgroundTaskIsNotRunning_WhenGettingPrice_ThenTaskStarts()
        {
            // Arrange
            _priceHubPoolMock.Setup(m => m.IsRunning(CryptoCurrency, Currency)).Returns(false);

            // Act
            await _controller.GetPrice(CryptoCurrency, Currency);

            // Assert
            _priceHubPoolMock.Verify(m => m.Start(CryptoCurrency, Currency), Times.Once);
        }

        [Fact]
        public async Task GivenBackgroundTaskIsRunning_WhenGettingPrice_ThenTaskDontStart()
        {
            // Arrange
            _priceHubPoolMock.Setup(m => m.IsRunning(CryptoCurrency, Currency)).Returns(true);

            // Act
            await _controller.GetPrice(CryptoCurrency, Currency);

            // Assert
            _priceHubPoolMock.Verify(m => m.Start(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GivenBackgroundTaskIsEitherRunningOrNot_WhenGettingPrice_ThenGetsPriceFromProvider(bool isRunning)
        {
            // Arrange
            var dto = new PriceDto();
            _priceHubPoolMock.Setup(m => m.IsRunning(CryptoCurrency, Currency)).Returns(isRunning);
            _cryptoProviderMock.Setup(m => m.GetPrice(CryptoCurrency, Currency)).Returns(Task.FromResult(dto));

            // Act
            var result = await _controller.GetPrice(CryptoCurrency, Currency);

            // Assert
            result.Should().NotBeNull();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeSameAs(dto);
        }
    }
}