using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Natcore.AspNet
{
	public interface IRequestDispatcher
	{
		Task<ActionResult> DispatchAsync(IRequest request);

		Task<ActionResult> DispatchAsync<TResult>(IRequest<TResult> request);
	}
}
