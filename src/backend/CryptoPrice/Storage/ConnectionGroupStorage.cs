using System.Collections.Generic;
using System.Threading;
using CryptoPrice.Hubs;
using Microsoft.EntityFrameworkCore.Internal;

namespace CryptoPrice.Storage
{
    public class ConnectionGroupStorage : IConnectionGroupStorage
    {
        private readonly IDictionary<string, List<string>> _connectionsByGroup;
        
        public ConnectionGroupStorage(IDictionary<string, List<string>> connectionsByGroup)
        {
            _connectionsByGroup = connectionsByGroup;
        }

        public void Store(string connectionId, string groupName)
        {
            if (!_connectionsByGroup.ContainsKey(groupName))
            {
                _connectionsByGroup.Add(groupName, new List<string> { connectionId });
            }
            else
            {
                _connectionsByGroup[groupName].Add(connectionId);
            }
        }

        public void Remove(string connectionId, string groupName)
        {
            if (_connectionsByGroup.ContainsKey(groupName))
            {
                _connectionsByGroup[groupName].Remove(connectionId);
            }
        }

        public void Remove(string groupName)
        {
            if (_connectionsByGroup.ContainsKey(groupName))
            {
                _connectionsByGroup.Remove(groupName);
            }
        }

        public bool IsEmpty(string groupName)
        {
            return !_connectionsByGroup[groupName].Any();
        }

        public void Lock()
        {
            Monitor.Enter(this);
        }

        public void Release()
        {
            Monitor.Exit(this);
        }
    }
}