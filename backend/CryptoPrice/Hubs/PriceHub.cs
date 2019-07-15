using System.Threading.Tasks;
using CryptoPrice.Storage;
using Microsoft.AspNetCore.SignalR;

namespace CryptoPrice.Hubs
{
    public class PriceHub : Hub<IPriceHub>
    {
        private readonly IConnectionGroupStorage _connectionGroupStorage;

        public PriceHub(IConnectionGroupStorage connectionGroupStorage)
        {
            _connectionGroupStorage = connectionGroupStorage;
        }

        public Task JoinGroup(string crypto, string currency)
        {
            var groupName = GetKey(crypto, currency);
            _connectionGroupStorage.Lock();
            _connectionGroupStorage.Store(Context.ConnectionId, groupName);
            _connectionGroupStorage.Release();
            return base.Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string crypto, string currency)
        {
            var groupName = GetKey(crypto, currency);
            _connectionGroupStorage.Lock();
            _connectionGroupStorage.Remove(Context.ConnectionId, groupName);
            _connectionGroupStorage.Release();
            return base.Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public static string GetKey(string crypto, string currency)
        {
            return $"{crypto}-{currency}";
        }
    }
}