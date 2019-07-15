namespace CryptoPrice.Storage
{
    public interface IConnectionGroupStorage
    {
        void Store(string connectionId, string groupName);
        void Remove(string connectionId, string groupName);
        void Remove(string groupName);
        bool IsEmpty(string groupName);
        void Lock();
        void Release();
    }
}