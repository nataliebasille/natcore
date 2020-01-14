using System.Linq;
using System.Threading.Tasks;

namespace Natcore.Core.Storage
{
	public interface IStorage
	{
		IQueryable<TEntity> Query<TEntity>() where TEntity : class;
		TEntity Add<TEntity>(TEntity entity) where TEntity : class;
		Task SaveChangesAsync();
	}
}
