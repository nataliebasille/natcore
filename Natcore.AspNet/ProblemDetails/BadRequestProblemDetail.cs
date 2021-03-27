using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Natcore.AspNet.ProblemDetails
{
	public class BadRequestProblemDetail : ProblemDetail
	{
		protected BadRequestProblemDetail()
			: base(400)
		{
			Title = "Validation Errors";
			Detail = "Some values in the request has failed validation";
		}

		public BadRequestProblemDetail(ModelStateDictionary modelState)
			: this()
		{
			Errors = modelState
				.Where(x => x.Value.Errors.Count > 0)
				.ToDictionary(x => x.Key, x => x.Value.Errors);
		}

		public BadRequestProblemDetail(object errors)
			: this()
		{
			Errors = errors;
		}

		public object Errors { get; set; }
	}
}
