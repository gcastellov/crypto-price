using System;
using System.Collections.Generic;
using CryptoPrice.Storage;
using FluentAssertions;
using Xunit;

namespace CryptoPrice.UnitTests.Storage
{
    public class ConnectionGroupStorageTests
    {
        private const string GroupName = "XRP-USD";

        private readonly ConnectionGroupStorage _connectionGroupStorage;
        private readonly Dictionary<string, List<string>> _connectionsByGroup;

        public ConnectionGroupStorageTests()
        {
            _connectionsByGroup = new Dictionary<string, List<string>>();
            _connectionGroupStorage = new ConnectionGroupStorage(_connectionsByGroup);
        }

        [Fact]
        public void GivenGroupNotExists_WhenStoringGroup_ThenAddsToStorage()
        {
            // Arrange
            var connectionId = Guid.NewGuid().ToString();

            // Act
            _connectionGroupStorage.Store(connectionId, GroupName);

            // Assert
            _connectionsByGroup.ContainsKey(GroupName).Should().BeTrue();
            _connectionsByGroup[GroupName].Should().Contain(connectionId);
        }

        [Fact]
        public void GivenGroupExists_WhenStoringGroup_ThenAddConnectionToGroup()
        {
            // Arrange
            var connectionId = Guid.NewGuid().ToString();
            _connectionsByGroup.Add(GroupName, new List<string> { Guid.NewGuid().ToString() });

            // Act
            _connectionGroupStorage.Store(connectionId, GroupName);

            // Assert
            _connectionsByGroup.ContainsKey(GroupName).Should().BeTrue();
            _connectionsByGroup[GroupName].Count.Should().Be(2);
            _connectionsByGroup[GroupName].Should().Contain(connectionId);
        }

        [Fact]
        public void GivenGroupExists_WhenRemovingConnection_ThenRemoveItFromGroup()
        {
            // Arrange
            var connectionId = Guid.NewGuid().ToString();
            _connectionsByGroup.Add(GroupName, new List<string> { connectionId });

            // Act
            _connectionGroupStorage.Remove(connectionId, GroupName);

            // Assert
            _connectionsByGroup[GroupName].Should().BeEmpty();
        }

        [Fact]
        public void GivenGroupNotExists_WhenRemovingConnection_ThenDoNothing()
        {
            // Arrange
            var connectionId = Guid.NewGuid().ToString();
            _connectionsByGroup.Add(GroupName, new List<string> { connectionId });
            var previousStorage = new Dictionary<string, List<string>>(_connectionsByGroup);

            // Act
            _connectionGroupStorage.Remove(Guid.NewGuid().ToString(), "NewGroup");

            // Assert
            _connectionsByGroup.Should().BeEquivalentTo(previousStorage);
        }

        [Fact]
        public void GivenGroupExists_WhenRemovingGroup_ThenRemovesIt()
        {
            // Arrange
            _connectionsByGroup.Add(GroupName, new List<string>());

            // Act
            _connectionGroupStorage.Remove(GroupName);

            // Assert
            _connectionsByGroup.Should().BeEmpty();
        }


        [Fact]
        public void GivenGroupNotExists_WhenRemovingGroup_ThenDoNothing()
        {
            // Arrange
            _connectionsByGroup.Add(GroupName, new List<string>());
            var previousStorage = new Dictionary<string, List<string>>(_connectionsByGroup);

            // Act
            _connectionGroupStorage.Remove("NewGroupName");

            // Assert
            _connectionsByGroup.Should().BeEquivalentTo(previousStorage);
        }
    }
}