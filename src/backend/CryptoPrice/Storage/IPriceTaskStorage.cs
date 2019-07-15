using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrice.Storage
{
    public interface IPriceTaskStorage
    {
        void Add(string key, Task task, CancellationTokenSource cancellationTokenSource);
        void Remove(string key);
        bool Exists(string key);
        (Task, CancellationTokenSource) Get(string key);
        IEnumerable<string> GetTaskKeys();
    }
}