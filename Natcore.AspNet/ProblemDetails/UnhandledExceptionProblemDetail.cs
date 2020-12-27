using System;
using System.Diagnostics;
using System.Linq;

namespace Natcore.AspNet.ProblemDetails
{
	public class UnhandledExceptionProblemDetail : ProblemDetail
	{
		public UnhandledExceptionProblemDetail(Exception ex)
			: base(500)
		{
			Title = "Unhandled Error";
			Detail = $"Oops, an unexpected error occurred{Environment.NewLine}Please contact the administrator";

#if DEBUG
			Message = ex.Message;
			Raw = ex.ToString();
			StackFrames = new StackTrace(ex, true).GetFrames().Select(x => new StackFrame
			{
				Filename = x.GetFileName(),
				LineNumber = x.GetFileLineNumber(),
				Method = x.GetMethod().Name
			})
			.ToArray();
#endif
		}

		public string ReferenceID { get; set; }

		public string Instance { get; set; }

		public string Message { get; }

		public string Raw { get; }

		public StackFrame[] StackFrames { get; }

		public class StackFrame
		{
			public int LineNumber { get; set; }
			public string Filename { get; set; }
			public string Method { get; set; }
		}
	}
}
