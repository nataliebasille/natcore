using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.AspNet.ExceptionHandling
{
	public interface IErrorIdGenerator
	{
		string NextID();
	}
}
