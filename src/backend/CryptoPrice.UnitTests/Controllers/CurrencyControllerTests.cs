using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoPrice.Controllers;
using CryptoPrice.CryptoProviders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CryptoPrice.UnitTests.Controllers
{
    public class CurrencyControllerTests
    {
        private readonly CurrencyController _controller;
        private readonly Mock<ICryptoProvider> _cryptoProviderMock;

        public CurrencyControllerTests()
        {
            _cryptoProviderMock = new Mock<ICryptoProvider>();
            _controller = new CurrencyController(_cryptoProviderMock.Object);
        }

        [Fact]
        public async Task WhenGettingCurrencies_ThenRetrieveThemFromProvider()
        {
            // Arrange
            IEnumerable<string> currencies = new[] {"USD", "EUR"};
            _cryptoProviderMock.Setup(m => m.GetSupportedCurrencies()).Returns(Task.FromResult(currencies));

            // Act
            var result = await _controller.GetCurrencies();

            // Assert
            _cryptoProviderMock.Verify(m => m.GetSupportedCurrencies(), Times.Once);
            result.Should().NotBeNull();
            var okResult = (OkObjectResult) result;
            okResult.Value.Should().BeSameAs(currencies);
        }

        [Fact]
        public async Task WhenGettingCryptoCurrencies_ThenRetrieveThemFromProvider()
        {
            // Arrange
            IEnumerable<string> currencies = new[] { "BTC", "XRP" };
            _cryptoProviderMock.Setup(m => m.GetSupportedCryptoCurrencies()).Returns(Task.FromResult(currencies));

            // Act
            var result = await _controller.GetCryptoCurrencies();

            // Assert
            _cryptoProviderMock.Verify(m => m.GetSupportedCryptoCurrencies(), Times.Once);
            result.Should().NotBeNull();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeSameAs(currencies);
        }
    }
}