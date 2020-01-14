using Newtonsoft.Json;

namespace Natcore.Core.Rest
{
	/// <summary>
	/// Some wholesome extensions for the <see cref="IResponse"/>
	/// </summary>
	public static class ResponseExtensions
	{
		/// <summary>
		/// A friendly way to deserialize the response body to that of any type you would like.
		/// </summary>
		/// <typeparam name="T">
		/// A curious little type that represents the desired response body data.
		/// </typeparam>
		/// <param name="response">
		/// A response that longs to be converted to response that has a deserialized body.
		/// </param>
		/// <returns>
		/// An <see cref="IResponse"/> with the Body deserialized to that of type <typeparamref name="T"/>
		/// </returns>
		public static IResponse<T> Deserialize<T>(this IResponse response)
			=> new RestResponse<T>()
			{
				StatusCode = response.StatusCode,
				Body = response.Body == null ? default : JsonConvert.DeserializeObject<T>(response.Body)
			};

		/// <summary>
		/// A friendly way to deserialize the response body to that of any type you would like.
		/// </summary>
		/// <typeparam name="T">
		/// A curious little type that represents the desired response body data.
		/// </typeparam>
		/// <param name="response">
		/// A response that longs to be converted to response that has a deserialized body.
		/// </param>
		/// <param name="anonymousObject">An anonymous object that is used to represent the data structure of the response body</param>
		/// <returns>
		/// An <see cref="IResponse"/> with the Body deserialized to that of type <typeparamref name="T"/>
		/// </returns>
		public static IResponse<T> Deserialize<T>(this IResponse response, T anonymousObject)
		{
			return Deserialize<T>(response);
		}

		/// <summary>
		/// Checks if the status code of a response is a success
		/// </summary>
		/// <param name="response">
		/// A response
		/// </param>
		/// <returns>
		/// true if the response status code is between 200 and 299 inclusively, otherwise false
		/// </returns>
		public static bool IsSuccess(this IResponse response)
			=> (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;
	}
}
