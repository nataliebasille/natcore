using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Natcore.Core.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Natcore.EntityFramework
{
	public class EfCoreStorage : IStorage, ISynchronousStorage, IStorageRemoval
	{
		private readonly DbContext _db;
		public EfCoreStorage(DbContext db)
		{
			_db = db;
		}

		public IQueryable<TEntity> Query<TEntity>() where TEntity : class
			=> _db.Set<TEntity>();

		public TEntity Add<TEntity>(TEntity entity) where TEntity : class
		{
			EntityEntry<TEntity> entry = _db.Set<TEntity>().Add(entity);
			return entry.Entity;
		}

		public Task SaveChangesAsync()
			=> _db.SaveChangesAsync();

		public void Remove(object entity)
			=> _db.Remove(entity);

		public void RemoveRange(IEnumerable<object> entities)
			=> _db.RemoveRange(entities);

		public void SaveChanges()
			=> _db.SaveChanges();
	}
}
