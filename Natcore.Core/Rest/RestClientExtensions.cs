using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Natcore.Core.Rest
{
	/// <summary>
	/// Extensions the rest client
	/// </summary>
	public static class RestClientExtensions
	{
		/// <summary>
		/// Send a request and get a response object with the data deserialized to the type of <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="request">The request information</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse<T>> SendAsync<T>(this IRestClient client, IRequest request)
			=> SendAsync(client, request, default(T));

		/// <summary>
		/// Send a request and get a response object with the data deserialized to the same type of <paramref name="anonymousObject"/>.
		/// </summary>
		/// <typeparam name="T">An anonymous type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="request">The request information</param>
		/// <param name="anonymousObject">An anonymous object that is used to represent the data structure of the response body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static async Task<IResponse<T>> SendAsync<T>(this IRestClient client, IRequest request, T anonymousObject)
		{
			if(client == null)
				throw new ArgumentException(nameof(client));

			if(request == null)
				throw new ArgumentException(nameof(request));

			return GetResponse<T>(await client.SendAsync(request));
		}

		/// <summary>
		/// Executes a GET request to the <paramref name="url"/>
		/// </summary>
		/// <param name="client">The rest client to use</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse> GetAsync(this IRestClient client, string url)
			=> Execute(client, new RestRequest
			{
				Url = url,
				Method = HttpMethod.Get
			});

		/// <summary>
		/// Executes a POST request to the <paramref name="url"/>
		/// </summary>
		/// <param name="client">The rest client to use</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="data">The data to send in the request body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse> PostAsync(this IRestClient client, string url, object data)
			=> Execute(client, new RestRequest
			{
				Url = url,
				Method = HttpMethod.Post,
				Body = data
			});

		/// <summary>
		/// Executes a PUT request to the <paramref name="url"/>
		/// </summary>
		/// <param name="client">The rest client to use</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="data">The data to send in the request body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse> PutAsync(this IRestClient client, string url, object data)
			=> Execute(client, new RestRequest
			{
				Url = url,
				Method = HttpMethod.Put,
				Body = data
			});

		/// <summary>
		/// Executes a DELETE request to the <paramref name="url"/>
		/// </summary>
		/// <param name="client">The rest client to use</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse> DeleteAsync(this IRestClient client, string url)
			=> Execute(client, new RestRequest
			{
				Url = url,
				Method = HttpMethod.Delete
			});

		/// <summary>
		/// Executes a GET request and returns a response object with the data deserialized to the type of <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse<T>> GetAsync<T>(this IRestClient client, string url)
			=> GetAsync(client, url, default(T));

		/// <summary>
		/// Executes a GET request and returns a response object with the data deserialized to the same type of <paramref name="anonymousObject"/>.
		/// </summary>
		/// <typeparam name="T">An anonymous type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="anonymousObject">An anonymous object that is used to represent the data structure of the response body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static async Task<IResponse<T>> GetAsync<T>(this IRestClient client, string url, T anonymousObject)
			=> GetResponse<T>(await GetAsync(client, url));

		/// <summary>
		/// Executes a POST request and returns a response object with the data deserialized to the type of <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="data">The data to send in the request body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse<T>> PostAsync<T>(this IRestClient client, string url, object data)
			=> PostAsync(client, url, data, default(T));


		/// <summary>
		/// Executes a POST request and returns a response object with the data deserialized to the same type of <paramref name="anonymousObject"/>.
		/// </summary>
		/// <typeparam name="T">An anonymous type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="data">The data to send in the request body</param>
		/// <param name="anonymousObject">An anonymous object that is used to represent the data structure of the response body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static async Task<IResponse<T>> PostAsync<T>(this IRestClient client, string url, object data, T anonymousObject)
			=> GetResponse<T>(await PostAsync(client, url, data));

		/// <summary>
		/// Executes a PUT request and returns a response object with the data deserialized to the type of <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="data">The data to send in the request body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse<T>> PutAsync<T>(this IRestClient client, string url, object data)
			=> PutAsync(client, url, data, default(T));

		/// <summary>
		/// Executes a PUT request and returns a response object with the data deserialized to the same type of <paramref name="anonymousObject"/>.
		/// </summary>
		/// <typeparam name="T">An anonymous type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="data">The data to send in the request body</param>
		/// <param name="anonymousObject">An anonymous object that is used to represent the data structure of the response body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static async Task<IResponse<T>> PutAsync<T>(this IRestClient client, string url, object data, T anonymousObject)
			=> GetResponse<T>(await PutAsync(client, url, data));

		/// <summary>
		/// Executes a DELETE request and returns a response object with the data deserialized to the type of <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static Task<IResponse<T>> DeleteAsync<T>(this IRestClient client, string url)
			=> DeleteAsync(client, url, default(T));

		/// <summary>
		/// Executes a DELETE request and returns a response object with the data deserialized to the same type of <paramref name="anonymousObject"/>.
		/// </summary>
		/// <typeparam name="T">An anonymous type that represents the format of the response body</typeparam>
		/// <param name="client">The rest client</param>
		/// <param name="url">The endpoint to send the request to</param>
		/// <param name="anonymousObject">An anonymous object that is used to represent the data structure of the response body</param>
		/// <returns>
		/// A task that when completed contains the response from the rest client
		/// </returns>
		public static async Task<IResponse<T>> DeleteAsync<T>(this IRestClient client, string url, T anonymousObject)
			=> GetResponse<T>(await DeleteAsync(client, url));

		private static Task<IResponse> Execute(IRestClient client, IRequest request)
		{
			if (client == null)
				throw new ArgumentException(nameof(client));

			if (request == null)
				throw new ArgumentException(nameof(request));

			return client.SendAsync(request);
		}

		private static IResponse<T> GetResponse<T>(IResponse response)
			=> response.IsSuccess()
				? response.Deserialize<T>()
				: new RestResponse<T>
				{
					StatusCode = response.StatusCode,
					Body = default
				};
	}
}
