using System.Collections.Generic;

namespace Tools
{
	public class VariantPool<TKey, IItem> : IVariantPool<TKey, IItem>
	{
		private readonly Dictionary<TKey, IPool<IItem>> _pools = new Dictionary<TKey, IPool<IItem>>();

		private readonly Dictionary<IItem, TKey> _busyItems = new Dictionary<IItem, TKey>();
        
		public void AddVariant(TKey variant, IPool<IItem> pool)
		{
			_pools.Add(variant, pool);
		} 

		public void RemoveVariant(TKey variant)
		{
			_pools.Remove(variant);
		}

		public IItem Get(TKey variant)
		{
			IItem item = _pools[variant].Get();
			_busyItems.Add(item, variant);
			return item;
		}

		public void Return(IItem item)
		{
			TKey variant = _busyItems[item];
			_pools[variant].Return(item);
			_busyItems.Remove(item);
		}

		public void ReturnAll()
		{
			foreach (var pool in _pools)
				pool.Value.ReturnAll();
			_busyItems.Clear();
		}
	}
}