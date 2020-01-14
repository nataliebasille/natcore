using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Natcore.Core.Rest
{
	/// <summary>
	/// Send http requests to any server your heart desires... and that
	/// can actually be reached from your computer.
	/// </summary>
	public class RestClient : IRestClient
	{
		private readonly string _baseUrl;
		private readonly HttpClient _external;

		/// <summary>
		/// Construct a new client.
		/// </summary>
		public RestClient() : this("") { }

		/// <summary>
		/// Construct a new client where every url is prepended with <paramref name="baseUrl"/>
		/// </summary>
		/// <param name="baseUrl">
		/// A prefix for every url sent through this client.
		/// </param>
		public RestClient(string baseUrl)
		{
			_baseUrl = baseUrl;

			if (_baseUrl != null && _baseUrl.Length > 0 && _baseUrl[_baseUrl.Length - 1] != '/')
				_baseUrl += '/';
		}

		public RestClient(HttpClient httpClient)
		{
			_external = httpClient;
		}

		/// <summary>
		/// Sometimes (especially in development) the server not having a valid certificate when using https can get 
		/// in the way of a healthy experience.  To avoid any and all invalid certificate errors
		/// set this property to true.
		/// </summary>
		public bool AllowInsecureSSL { get; set; }

		/// <summary>
		/// Sends a request to the specified url and returns a <see cref="IResponse"/>.
		/// </summary>
		/// <param name="request">
		/// The request.
		/// </param>
		/// <returns>
		/// A response from the server... maybe; I guess a 404 response comes from the internal workings
		/// of this method.
		/// </returns>
		public async virtual Task<IResponse> SendAsync(IRequest request)
		{
			HttpResponseMessage httpResponse = null;
			HttpClient client = CreateClient(out Action disposer);

			bool isStringContent = request.Body != null && request.Body.GetType() == typeof(string);
			HttpRequestMessage requestMessage = new HttpRequestMessage(request.Method, request.Url)
			{
				Content =
					request.Body is HttpContent
					? request.Body as HttpContent
					: request.Body != null
					? new StringContent(
						isStringContent
							? (string)request.Body
							: JsonConvert.SerializeObject(request.Body),
						System.Text.Encoding.UTF8,
						(request.Headers?.ContainsKey("Content-Type") ?? false)
							? request.Headers["Content-Type"]
							: isStringContent
							? "text/plain"
							: "application/json"
					)
					: null
			};

			foreach (KeyValuePair<string, string> pair in (request.Headers?.ToList() ?? new List<KeyValuePair<string, string>>()))
				requestMessage.Headers.TryAddWithoutValidation(pair.Key, pair.Value);

			httpResponse = await client.SendAsync(requestMessage);

			disposer?.Invoke();

			var res = new RestResponse
			{
				StatusCode = httpResponse.StatusCode,
				Body = await httpResponse.Content.ReadAsStringAsync()
			};

			Action<string> logger = Debugger.IsAttached
					? str => Debug.WriteLine(str)
					: (Action<string>)Console.WriteLine;

			logger($"Status: {res.StatusCode}");
			logger($"Body: {res.Body}");

			return res;
		}

		/// <summary>
		/// Override this method if you want to create your own http client
		/// </summary>
		/// <param name="disposer">
		/// An action that handles how to dispose the http client.
		/// </param>
		/// <returns>
		/// An http client
		/// </returns>
		protected virtual HttpClient CreateClient(out Action disposer)
		{
			if(_external != null)
			{
				disposer = () => { };
				return _external;
			}

			var handler = new HttpClientHandler();
			var client = new HttpClient(handler, true)
			{
				BaseAddress = new Uri(_baseUrl)
			};

			if (AllowInsecureSSL)
				handler.ServerCertificateCustomValidationCallback =
					(sender, certificate, chain, sslPolicyErrors) => true;

			disposer = () => client.Dispose();

			return client;
		}
	}
}
