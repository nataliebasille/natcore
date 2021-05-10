using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Natcore.AspNet
{
	public interface IRequest { }

	public interface IRequest<out TResult> { }

    public interface IRequestHandler<in TRequest>
		where TRequest : IRequest
    {
        Task<ActionResult> HandleAsync(TRequest request);
    }

	public interface IRequestHandler<in TRequest, TResult>
		where TRequest : IRequest<TResult>
	{
		Task<ActionResult<TResult>> HandleAsync(TRequest request);
	}
}
