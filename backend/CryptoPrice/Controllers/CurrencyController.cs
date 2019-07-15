using System.Threading.Tasks;
using CryptoPrice.CryptoProviders;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPrice.Controllers
{
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICryptoProvider _cryptoProvider;

        public CurrencyController(ICryptoProvider cryptoProvider)
        {
            _cryptoProvider = cryptoProvider;
        }

        [Route("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await _cryptoProvider.GetSupportedCurrencies();
            return Ok(currencies);
        }

        [Route("crypto-currencies")]
        public async Task<IActionResult> GetCryptoCurrencies()
        {
            var cryptoCurrencies = await _cryptoProvider.GetSupportedCryptoCurrencies();
            return Ok(cryptoCurrencies);
        }
    }
}