using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoPrice.Dtos;

namespace CryptoPrice.CryptoProviders
{
    public interface ICryptoProvider
    {
        Task<PriceDto> GetPrice(string cryptoCurrency, string currency);
        Task<IEnumerable<string>> GetSupportedCurrencies();
        Task<IEnumerable<string>> GetSupportedCryptoCurrencies();
    }
}