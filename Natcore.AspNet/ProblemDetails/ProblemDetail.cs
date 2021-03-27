using Newtonsoft.Json;
using System.Collections.Generic;

namespace Natcore.AspNet.ProblemDetails
{
	public class ProblemDetail
	{
		public ProblemDetail(int status)
		{
			Type = $"https://httpstatuses.com/{status}";
			Status = status;
		}

		public string Type { get; protected set; }

		public int Status { get; protected set; }

		public string Title { get; set; }

		public string Detail { get; set; }

		[JsonExtensionData]
		public Dictionary<string, object> Extensions { get; set; }
	}
}
