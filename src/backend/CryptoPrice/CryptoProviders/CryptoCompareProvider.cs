using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoPrice.Dtos;
using CryptoPrice.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoPrice.CryptoProviders
{
    public class CryptoCompareProvider : ICryptoProvider
    {
        private static readonly string CryptoProviderUrl = "https://min-api.cryptocompare.com/data/price";
        private static readonly string AuthHeader = "authorization";

        private readonly string _apiKey;

        public CryptoCompareProvider(string apiKey)
        {
            _apiKey = apiKey;
        }

        private static IDictionary<string, string> GetParameters(string cryptoCurrency, string currency)
        {
            return new Dictionary<string, string>
            {
                { "fsym", cryptoCurrency },
                { "tsyms", currency }
            };
        }

        public async Task<PriceDto> GetPrice(string cryptoCurrency, string currency)
        {
            var queryString = GetParameters(cryptoCurrency, currency);
            var uri = new Uri(CryptoProviderUrl).SetQueryString(queryString);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(AuthHeader, $"Apikey {_apiKey}");
            var response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            var json = (JObject)JsonConvert.DeserializeObject(content);            

            return new PriceDto
            {
                Crypto = cryptoCurrency,
                Currency = currency,
                Price = json.First.ToObject<double>(),
                At = DateTime.UtcNow
            };
        }

        public Task<IEnumerable<string>> GetSupportedCurrencies()
        {
            IEnumerable<string> currencies = new [] {"USD", "EUR"};
            return Task.FromResult(currencies);
        }

        public Task<IEnumerable<string>> GetSupportedCryptoCurrencies()
        {
            IEnumerable<string> cryptoCurrencies = new[] {"BTC", "XRP"};
            return Task.FromResult(cryptoCurrencies);
        }
    }
}