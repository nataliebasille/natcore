using System.Collections.Generic;

namespace Natcore.Core.Storage
{
	public interface IStorageRemoval
	{
		void Remove(object entity);
		void RemoveRange(IEnumerable<object> entities);
	}
}
