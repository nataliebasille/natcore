using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Natcore.AspNet
{
    public class ModelValidationRequestHandler<TParams> : IRequestHandler<TParams>
    {
        private readonly IRequestHandler<TParams> _handler;
        private readonly ModelStateDictionary _modelState;

        public ModelValidationRequestHandler(IRequestHandler<TParams> handler, ModelStateDictionary modelState)
        {
            _handler = handler;
            _modelState = modelState;
        }

        public Task<IActionResult> HandleAsync(TParams request)
        {
            if (_modelState != null && !_modelState.IsValid)
                return Task.FromResult(RequestResult.BadRequest(ToErrorObject(_modelState)));

            return _handler.HandleAsync(request);
        }

        private object ToErrorObject(ModelStateDictionary modelState)
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
