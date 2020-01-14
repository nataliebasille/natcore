using System.Collections.Generic;
using System.Net.Http;

namespace Natcore.Core.Rest
{
	public class RestRequest : IRequest
	{
		public IDictionary<string, string> Headers { get; set; }
		public string Url { get; set; }
		public HttpMethod Method { get; set; }
		public object Body { get; set; }
	}
}
