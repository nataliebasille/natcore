using System.Net;

namespace Natcore.Core.Rest
{
	/// <summary>
	/// An informative infogram of the information returned from an http request.
	/// </summary>
	public interface IResponse
	{
		/// <summary>
		/// The HTTP protocol has a concept of a status code.  You should probably do a quick google search
		/// if you don't know what the status code means.  A 2xx status code indicates the request was successful;
		/// anything else, is up for grabs.
		/// </summary>
		HttpStatusCode StatusCode { get; }

		/// <summary>
		/// What ever data the server wants to return.
		/// </summary>
		string Body { get; }
	}

	public interface IResponse<T>
	{
		HttpStatusCode StatusCode { get; }

		T Body { get; }
	}
}
