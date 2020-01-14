using Microsoft.AspNetCore.Mvc;

namespace Natcore.AspNet.Testing.ActionResultAssertions
{
	public class ResultAssertion
	{
		public ResultAssertion(IActionResult result)
		{
			Subject = result;
			Body = (result as ObjectResult)?.Value;
		}

		public IActionResult Subject { get; }

		public object Body { get; }
	}
}
