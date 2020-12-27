using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Natcore.AspNet.ProblemDetails
{
	public interface IProblemDetailFactory
	{
		ProblemDetail CreateDetail(HttpContext context, Exception error);
	}

	public class ProblemDetailFactory : IProblemDetailFactory
	{
		private readonly IOptions<ProblemDetailsOptions> _options;
		private readonly IDetailIdGenerator _idGenerator;

		public ProblemDetailFactory(IOptions<ProblemDetailsOptions> options, IDetailIdGenerator idGenerator)
		{
			_options = options;
			_idGenerator = idGenerator;
		}

		public ProblemDetail CreateDetail(HttpContext context, Exception error)
		{
			var detail = error != null ? CreateFromError(error) : CreateFromResponse(context);

			detail.Extensions.Add("ReferenceId", _idGenerator.NextID());

			return detail;
		}

		private UnhandledExceptionProblemDetail CreateFromError(Exception error)
		{
			return new UnhandledExceptionProblemDetail(error);
		}

		private ProblemDetail CreateFromResponse(HttpContext context)
		{
			using (var stream = new MemoryStream())
			{
				context.Response.Body = stream;

				stream.Position = 0;
				string response = new StreamReader(stream).ReadToEnd();
				Dictionary<string, object> extensions = null;

				response = response?.Trim();
				if (!string.IsNullOrWhiteSpace(response) && (response.StartsWith("{") && response.EndsWith("}")) ||
					(response.StartsWith("[") && response.EndsWith("]")))
				{
					try
					{
						var token = JToken.Parse(response);

						if (token is JArray)
							extensions = new Dictionary<string, object> { ["Errors"] = token };

						if (token is JObject)
							extensions = token.ToObject<Dictionary<string, object>>();

					}
					catch (Exception) { }

					if (extensions == null)
						extensions = new Dictionary<string, object> { ["Error"] = response };
				}

				extensions ??= new Dictionary<string, object>();

				if (!extensions.ContainsKey("Title"))
					extensions.Add("Title", ReasonPhrases.GetReasonPhrase(context.Response.StatusCode));

				if (!extensions.ContainsKey("Detail"))
					extensions.Add("Detail", ReasonPhrases.GetReasonPhrase(context.Response.StatusCode));

				return new ProblemDetail(context.Response.StatusCode)
				{
					Extensions = extensions
				};
			}
		}
	}
}
