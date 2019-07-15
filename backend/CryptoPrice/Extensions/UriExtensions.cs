using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoPrice.Extensions
{
    internal static class UriExtensions
    {
        public static Uri SetQueryString(this Uri uri, IDictionary<string, string> parameters)
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append(uri.AbsoluteUri);

            if (parameters != null && parameters.Any())
            {
                sBuilder.Append('?');

                for (int i = 0; i < parameters.Count; i++)
                {
                    if (i != 0)
                    {
                        sBuilder.Append('&');
                    }

                    var key = parameters.Keys.ElementAt(i);
                    sBuilder.Append(key);
                    sBuilder.Append('=');
                    sBuilder.Append(parameters[key]);
                }                
            }

            return new Uri(sBuilder.ToString());
        }
    }
}