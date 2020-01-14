using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.Core.Rest
{
	public static class RequestExtensions
	{
		public static IRequest SetHeader(this IRequest request, string key, string value)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (request.Headers == null)
				request.Headers = new Dictionary<string, string>();

			if (request.Headers.ContainsKey(key))
				request.Headers[key] = value;
			else
				request.Headers.Add(key, value);

			return request;
		}
	}
}
