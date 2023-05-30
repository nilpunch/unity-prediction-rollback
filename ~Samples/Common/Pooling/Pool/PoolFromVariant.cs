namespace UPR.Samples
{
	/// <summary>
	/// Adapter from VariantPool to basic Pool.
	/// </summary>
	public class PoolFromVariant<TVariant, TItem> : IPool<TItem>
	{
		private readonly TVariant _variant;
		private readonly IVariantPool<TVariant, TItem> _variantPool;

		public PoolFromVariant(TVariant variant, IVariantPool<TVariant, TItem> variantPool)
		{
			_variant = variant;
			_variantPool = variantPool;
		}

		public void Return(TItem item)
		{
			_variantPool.Return(item);
		}

		public TItem Get()
		{
			return _variantPool.Get(_variant);
		}
	}
}
