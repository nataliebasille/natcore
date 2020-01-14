using System.Threading.Tasks;

namespace Natcore.Core.Rest
{
	public interface IRestClient
	{
		Task<IResponse> SendAsync(IRequest request);
	}
}
