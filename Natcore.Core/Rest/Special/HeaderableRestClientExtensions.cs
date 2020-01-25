using System;
using System.Collections.Generic;

namespace Natcore.Core.Rest
{
    public static class HeaderableRestClientExtensions
    {
        public static HeaderableRestClient SetHeader(this HeaderableRestClient restClient, string key, string value)
		{
			if (restClient == null)
				throw new ArgumentNullException(nameof(restClient));

			if (restClient.Headers == null)
				restClient.Headers = new Dictionary<string, string>();

			if (restClient.Headers.ContainsKey(key))
				restClient.Headers[key] = value;
			else
				restClient.Headers.Add(key, value);

			return restClient;
		}
	}
}
