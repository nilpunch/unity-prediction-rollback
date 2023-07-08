using System.Collections.Generic;

namespace UPR.Useful
{
	public class VariantPool<TKey, TItem> : IVariantPool<TKey, TItem>
	{
		private readonly Dictionary<TKey, IPool<TItem>> _pools = new Dictionary<TKey, IPool<TItem>>();

		private readonly Dictionary<TItem, TKey> _busyItems = new Dictionary<TItem, TKey>();

		public void AddVariant(TKey variant, IPool<TItem> pool)
		{
			_pools.Add(variant, pool);
		}

		public void RemoveVariant(TKey variant)
		{
			_pools.Remove(variant);
		}

		public TItem Get(TKey variant)
		{
			TItem item = _pools[variant].Get();
			_busyItems.Add(item, variant);
			return item;
		}

		public void Return(TItem item)
		{
			TKey variant = _busyItems[item];
			_pools[variant].Return(item);
			_busyItems.Remove(item);
		}
	}
}
