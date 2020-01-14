namespace Natcore.Core.Storage
{
	public interface IDecorableStorage
	{
		IStorage BaseStorage { get; }
	}
}
