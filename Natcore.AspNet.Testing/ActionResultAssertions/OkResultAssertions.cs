using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;

namespace Natcore.AspNet.Testing.ActionResultAssertions
{
    public class OkResultAssertions
    {
		private readonly IActionResult _result;
		public OkResultAssertions(IActionResult result)
		{
			_result = result;
		}

		public OkResultAssertions WithBody(object body)
		{
			Execute.Assertion
				.ForCondition(_result is OkObjectResult)
				.FailWith("Expected result to have body, but none was found");

			(_result as OkObjectResult).Value.Should().BeEquivalentTo(body);

			return this;
		}
    }
}
