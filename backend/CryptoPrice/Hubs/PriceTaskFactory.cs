using System.Threading;
using System.Threading.Tasks;
using CryptoPrice.CryptoProviders;
using Microsoft.AspNetCore.SignalR;

namespace CryptoPrice.Hubs
{
    public class PriceTaskFactory : IPriceTaskFactory
    {
        private readonly IHubContext<PriceHub, IPriceHub> _hubContext;
        private readonly ICryptoProvider _cryptoProvider;

        public PriceTaskFactory(IHubContext<PriceHub, IPriceHub> hubContext, ICryptoProvider cryptoProvider)
        {
            _hubContext = hubContext;
            _cryptoProvider = cryptoProvider;
        }

        public Task CreatePollingPriceTask(string crypto, string currency, CancellationToken cancellationToken)
        {
            return Task.Run(() => CreatePollingPrice(crypto, currency, cancellationToken), cancellationToken);
        }

        private void CreatePollingPrice(string crypto, string currency, CancellationToken cancellationToken)
        {
            string groupName = PriceHub.GetKey(crypto, currency);

            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(10000);
                var response = _cryptoProvider.GetPrice(crypto, currency).GetAwaiter().GetResult();
                _hubContext.Clients.Group(groupName).SendPrice(response);
            }
        }
    }
}