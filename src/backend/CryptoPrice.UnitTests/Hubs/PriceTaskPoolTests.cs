using System.Threading;
using System.Threading.Tasks;
using CryptoPrice.Hubs;
using CryptoPrice.Storage;
using FluentAssertions;
using Moq;
using Xunit;

namespace CryptoPrice.UnitTests.Hubs
{
    public class PriceTaskPoolTests
    {
        private const string CryptoCurrency = "XRP";
        private const string Currency = "USD";
        private const string Key = "XRP-USD";

        private readonly PriceTaskPool _priceTaskPool;
        private readonly Mock<IPriceTaskStorage> _taskStorageMock;
        private readonly Mock<IPriceTaskFactory> _taskFactoryMock;

        public PriceTaskPoolTests()
        {
            _taskStorageMock = new Mock<IPriceTaskStorage>();
            _taskFactoryMock = new Mock<IPriceTaskFactory>();
            var priceTaskMock = new Mock<IPriceTask>();

            _priceTaskPool = new PriceTaskPool(
                _taskFactoryMock.Object,
                priceTaskMock.Object,
                _taskStorageMock.Object);
        }

        [Fact]
        public void GivenTaskNotExistsAtStorage_WhenIsRunning_ThenReturnsFalse()
        {
            // Arrange
            _taskStorageMock.Setup(m => m.Exists(Key)).Returns(false);

            // Act
            bool result = _priceTaskPool.IsRunning(CryptoCurrency, Currency);

            // Assert
            result.Should().BeFalse();
            _taskStorageMock.Verify(m => m.Exists(Key), Times.Once);
        }

        [Fact]
        public void GivenTaskExistsAtStorage_WhenIsRunning_ThenReturnsTrue()
        {
            // Arrange
            _taskStorageMock.Setup(m => m.Exists(Key)).Returns(true);

            // Act
            bool result = _priceTaskPool.IsRunning(CryptoCurrency, Currency);

            // Assert
            result.Should().BeTrue();
            _taskStorageMock.Verify(m => m.Exists(Key), Times.Once);
        }

        [Fact]
        public void GivenCryptoAndCurrency_WhenStarting_ThenCreatesTaskAndStoresIt()
        {
            // Arrange
            var task = Task.CompletedTask;
            _taskFactoryMock.Setup(m => m.CreatePollingPriceTask(CryptoCurrency, Currency, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            _priceTaskPool.Start(CryptoCurrency, Currency);

            // Assert
            _taskStorageMock.Verify(m => m.Add(Key, task, It.IsAny<CancellationTokenSource>()), Times.Once);
        }
    }
}