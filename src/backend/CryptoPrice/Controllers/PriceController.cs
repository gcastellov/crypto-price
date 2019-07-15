using System.Threading.Tasks;
using CryptoPrice.CryptoProviders;
using CryptoPrice.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPrice.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IPriceTaskPool _priceHubPool;

        public PriceController(ICryptoProvider cryptoProvider, IPriceTaskPool priceHubPool)
        {
            _cryptoProvider = cryptoProvider;
            _priceHubPool = priceHubPool;
        }

        public async Task<IActionResult> GetPrice(string crypto, string currency)
        {
            if (!_priceHubPool.IsRunning(crypto, currency))
            {
                _priceHubPool.Start(crypto, currency);
            }

            var response = await _cryptoProvider.GetPrice(crypto, currency);
            return Ok(response);
        }
    }
}