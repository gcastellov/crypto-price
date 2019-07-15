using System.Threading.Tasks;
using CryptoPrice.Dtos;

namespace CryptoPrice.Hubs
{
    public interface IPriceHub
    {
        Task SendPrice(PriceDto priceDto);
        Task JoinGroup(string crypto, string currency);
        Task LeaveGroup(string crypto, string currency);
    }
}