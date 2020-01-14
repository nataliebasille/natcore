using System;
using System.Collections.Generic;

namespace Natcore.Core.Storage
{
	public static class StorageExtensions
	{
		public static IStorage Remove(this IStorage storage, object entity)
			=> FindAndExecute<IStorageRemoval>(nameof(Remove), storage, r => r.Remove(entity), true);

		public static IStorage RemoveRange(this IStorage storage, params object[] entities)
			=> RemoveRange(storage, entities as IEnumerable<object>);

		public static IStorage RemoveRange(this IStorage storage, IEnumerable<object> entities)
			=> FindAndExecute<IStorageRemoval>(nameof(RemoveRange), storage, r => r.RemoveRange(entities), true);

		public static IStorage TryRemove(this IStorage storage, object entity)
			=> FindAndExecute<IStorageRemoval>(nameof(Remove), storage, r => r.Remove(entity), false);

		public static IStorage TryRemoveRange(this IStorage storage, params object[] entities)
			=> TryRemoveRange(storage, entities as IEnumerable<object>);

		public static IStorage TryRemoveRange(this IStorage storage, IEnumerable<object> entities)
			=> FindAndExecute<IStorageRemoval>(nameof(RemoveRange), storage, r => r.RemoveRange(entities), false);

		public static IStorage SaveChanges(this IStorage storage)
			=> FindAndExecute<ISynchronousStorage>(nameof(SaveChanges), storage, r => r.SaveChanges(), true);

		private static IStorage FindAndExecute<TInterface>(string method, IStorage storage, Action<TInterface> action, bool throwIfNotFound)
		{
			while (storage != null && storage is IDecorableStorage && !(storage is TInterface))
				storage = (storage as IDecorableStorage).BaseStorage;

			if (storage is TInterface)
				action((TInterface)storage);

			else if (throwIfNotFound)
				throw new InvalidOperationException($"Unable to use method {method} for storage type {storage.GetType()}.  Ensure storage class implements {typeof(TInterface)}");

			return storage;
		}
	}
}
