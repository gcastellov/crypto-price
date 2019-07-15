using System;
using System.Collections.Generic;
using System.Linq;
using CryptoPrice.Extensions;
using FluentAssertions;
using Xunit;

namespace CryptoPrice.UnitTests.Extensions
{
    public class UriExtensionsTests
    {
        [Fact]
        public void GivenUri_WhenAddingParameters_ThenBuildQueryString()
        {
            // Arrange
            const string url = "http://www.someurl.com/segment";
            var parameters = new Dictionary<string, string>
            {
                {"param1", "value1"},
                {"param2", "value2"}
            };

            var uri = new Uri(url);

            // Act
            var result = uri.SetQueryString(parameters);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be($"{url}?{parameters.Keys.ElementAt(0)}={parameters.Values.ElementAt(0)}&{parameters.Keys.ElementAt(1)}={parameters.Values.ElementAt(1)}");
        }
    }
}