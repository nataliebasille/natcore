using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.AspNet.ProblemDetails
{
	public class UnauthorizedProblemDetail : ProblemDetail	
	{
		public UnauthorizedProblemDetail()
			: base(401)
		{
			Title = "Unauthorized";
			Detail = "Sorry, you don't have the necessary permissions to view this resource";
		}
	}
}
