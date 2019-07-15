using System.Threading;
using CryptoPrice.Storage;

namespace CryptoPrice.Hubs
{
    public sealed class PriceTaskPool : IPriceTaskPool
    {
        private readonly IPriceTaskFactory _priceTaskFactory;
        private readonly IPriceTaskStorage _priceTaskStorage;

        public PriceTaskPool(IPriceTaskFactory priceTaskFactory, IPriceTask priceTask, IPriceTaskStorage priceTaskStorage)
        {
            _priceTaskFactory = priceTaskFactory;
            _priceTaskStorage = priceTaskStorage;
            priceTask.Start();
        }

        public bool IsRunning(string crypto, string currency)
        {
            string key = PriceHub.GetKey(crypto, currency);
            return _priceTaskStorage.Exists(key);
        }

        public void Start(string crypto, string currency)
        {
            string key = PriceHub.GetKey(crypto, currency);
            var cancellationTokenSource = new CancellationTokenSource();
            var task = _priceTaskFactory.CreatePollingPriceTask(crypto, currency, cancellationTokenSource.Token);
            _priceTaskStorage.Add(key, task, cancellationTokenSource);
        }
    }
}