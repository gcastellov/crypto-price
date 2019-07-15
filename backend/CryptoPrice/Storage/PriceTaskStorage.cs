using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrice.Storage
{
    public class PriceTaskStorage : IPriceTaskStorage
    {
        private readonly Dictionary<string, Task> _backgroundTasks;
        private readonly Dictionary<string, CancellationTokenSource> _cancellationTokenSources;

        public PriceTaskStorage(Dictionary<string, Task> backgroundTasks, Dictionary<string, CancellationTokenSource> cancellationTokenSources)
        {
            _backgroundTasks = backgroundTasks;
            _cancellationTokenSources = cancellationTokenSources;
        }

        public void Add(string key, Task task, CancellationTokenSource cancellationTokenSource)
        {
            _backgroundTasks.Add(key, task);
            _cancellationTokenSources.Add(key, cancellationTokenSource);
        }

        public void Remove(string key)
        {
            _backgroundTasks.Remove(key);
            _cancellationTokenSources.Remove(key);
        }

        public bool Exists(string key)
        {
            return _backgroundTasks.ContainsKey(key);
        }

        public (Task, CancellationTokenSource) Get(string key)
        {
            return new ValueTuple<Task, CancellationTokenSource>(_backgroundTasks[key], _cancellationTokenSources[key]);
        }

        public IEnumerable<string> GetTaskKeys()
        {
            return _backgroundTasks.Select(b => b.Key);
        }
    }
}