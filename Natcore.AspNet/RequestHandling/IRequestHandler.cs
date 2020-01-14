using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Natcore.AspNet
{
    public interface IRequestHandler<TQuery>
    {
        Task<IActionResult> HandleAsync(TQuery request);
    }
}
