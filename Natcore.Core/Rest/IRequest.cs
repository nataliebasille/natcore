using System.Collections.Generic;
using System.Net.Http;

namespace Natcore.Core.Rest
{
	public interface IRequest
	{
		HttpMethod Method { get; set; }

		string Url { get; set; }

		IDictionary<string, string> Headers { get; set; }

		object Body { get; set; }
	}
}
