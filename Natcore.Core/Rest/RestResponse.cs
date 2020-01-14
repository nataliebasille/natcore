using System.Net;

namespace Natcore.Core.Rest
{
	public class RestResponse : IResponse, IResponse<string>
	{
		public HttpStatusCode StatusCode { get; set; }

		public string Body { get; set; }
	}

	public class RestResponse<T> : IResponse<T>
	{
		public HttpStatusCode StatusCode { get; set; }

		public T Body { get; set; }
	}
}
