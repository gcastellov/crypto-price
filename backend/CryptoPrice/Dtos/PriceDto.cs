using System;

namespace CryptoPrice.Dtos
{
    public class PriceDto
    {
        public string Crypto { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
        public DateTime At { get; set; }
    }
}