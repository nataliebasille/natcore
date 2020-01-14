using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Natcore.AspNet
{
	public interface IRequestDispatcher
	{
		Task<IActionResult> DispatchAsync<TParams>(TParams parameters);
	}
}
