using Microsoft.AspNetCore.WebUtilities;

namespace Natcore.AspNet.ProblemDetails
{
	public class StatusCodeProblemDetail : ProblemDetail
	{
		public StatusCodeProblemDetail(int status, string reason)
			: base(status)
		{
			Title = ReasonPhrases.GetReasonPhrase(status);
			Detail = reason ?? Title;
		}
	}
}
