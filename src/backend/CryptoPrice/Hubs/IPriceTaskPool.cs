namespace CryptoPrice.Hubs
{
    public interface IPriceTaskPool
    {
        bool IsRunning(string crypto, string currency);
        void Start(string crypto, string currency);
    }
}