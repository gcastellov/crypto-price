using System;
using System.Linq;
using System.Threading;
using CryptoPrice.Storage;

namespace CryptoPrice.Hubs
{
    public sealed class PriceTask : IPriceTask, IDisposable
    {
        private const int PoolingTimeInMilliseconds = 10000;

        private readonly IConnectionGroupStorage _connectionGroupStorage;
        private readonly IPriceTaskStorage _priceTaskStorage;
        private readonly Thread _cleanerThread;
        private bool _isDisposing = false;

        public PriceTask(IConnectionGroupStorage connectionGroupStorage, IPriceTaskStorage priceTaskStorage)
        {
            _connectionGroupStorage = connectionGroupStorage;
            _priceTaskStorage = priceTaskStorage;
            var tStart = new ThreadStart(CheckNotUsedTasks);
            _cleanerThread = new Thread(tStart);
        }

        public void Start()
        {
            _cleanerThread.Start();
        }

        private void CheckNotUsedTasks()
        {
            while (!_isDisposing)
            {
                Thread.Sleep(PoolingTimeInMilliseconds);
                var taskKeys = _priceTaskStorage.GetTaskKeys().ToArray();

                if (taskKeys.Any())
                {
                    _connectionGroupStorage.Lock();

                    foreach (var taskKey in taskKeys)
                    {
                        if (_connectionGroupStorage.IsEmpty(taskKey))
                        {
                            CancelTask(taskKey);
                        }
                    }

                    _connectionGroupStorage.Release();
                }
            }
        }

        private void CancelTask(string taskKey)
        {
            var taskPair = _priceTaskStorage.Get(taskKey);
            taskPair.Item1.GetAwaiter().OnCompleted(() => CleanGroupResources(taskKey));
            taskPair.Item2.Cancel();
        }

        private void CleanGroupResources(string groupName)
        {
            _connectionGroupStorage.Remove(groupName);
            _priceTaskStorage.Remove(groupName);
        }

        public void Dispose()
        {
            _isDisposing = true;
        }
    }
}