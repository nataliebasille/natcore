using System;

namespace Natcore.AspNet.ProblemDetails
{
	public class DefaultDetailIdGenerator : IDetailIdGenerator
	{
		public string NextID() => DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss.fff");
	}
}
