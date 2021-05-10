using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Natcore.AspNet
{
    public class ModelValidationRequestHandler<TRequest> : IRequestHandler<TRequest>
		where TRequest : IRequest
    {
        private readonly IRequestHandler<TRequest> _handler;
        private readonly ModelStateDictionary _modelState;

        public ModelValidationRequestHandler(IRequestHandler<TRequest> handler, ModelStateDictionary modelState)
        {
            _handler = handler;
            _modelState = modelState;
        }

        public Task<ActionResult> HandleAsync(TRequest request)
        {
            if (_modelState != null && !_modelState.IsValid)
                return Task.FromResult(RequestResult.BadRequest(Helpers.ToErrorObject(_modelState)));

            return _handler.HandleAsync(request);
        }
    }

	public class ModelValidationRequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
		where TRequest : IRequest<TResult>
	{
		private readonly IRequestHandler<TRequest, TResult> _handler;
		private readonly ModelStateDictionary _modelState;

		public ModelValidationRequestHandler(IRequestHandler<TRequest, TResult> handler, ModelStateDictionary modelState)
		{
			_handler = handler;
			_modelState = modelState;
		}

		public Task<ActionResult<TResult>> HandleAsync(TRequest request)
		{
			if (_modelState != null && !_modelState.IsValid)
				return Task.FromResult((ActionResult<TResult>)RequestResult.BadRequest(Helpers.ToErrorObject(_modelState)));

			return _handler.HandleAsync(request);
		}
	}

	internal static class Helpers
	{
		internal static object ToErrorObject(ModelStateDictionary modelState)
		{
			var obj = new Dictionary<string, object>();

			foreach (var pair in modelState)
			{
				if (pair.Value.Errors.Count > 0)
					obj.Add(pair.Key, string.Join(Environment.NewLine, pair.Value.Errors.Select(x => x.ErrorMessage)));
			}

			return obj;
		}
	}
}
