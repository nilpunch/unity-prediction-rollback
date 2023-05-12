namespace Tools
{
	public interface IVariantPool<TVariant, TItem> : IPoolReturn<TItem>
	{
		void AddVariant(TVariant variant, IPool<TItem> pool);
		void RemoveVariant(TVariant variant);
		TItem Get(TVariant variant);
		void ReturnAll();
	}
}