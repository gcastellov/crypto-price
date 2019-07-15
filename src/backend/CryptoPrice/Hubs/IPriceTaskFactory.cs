using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrice.Hubs
{
    public interface IPriceTaskFactory
    {
        Task CreatePollingPriceTask(string crypto, string currency, CancellationToken cancellationToken);
    }
}