using System;
using System.Collections.Generic;

namespace UPR.Samples
{
    public class Pool<TItem> : IPool<TItem>
    {
        private readonly HashSet<TItem> _allCreatedItems = new HashSet<TItem>();
        private readonly Stack<TItem> _availableItems = new Stack<TItem>();

        private readonly IFactory<TItem> _factory;
        
        public Pool(IFactory<TItem> factory)
        {
            _factory = factory;
        }

        public Pool(IFactory<TItem> factory, int prewarm) : this(factory)
        {
            for (int i = 0; i < prewarm; i++)
            {
                TItem poolable = _factory.Create();
                _allCreatedItems.Add(poolable);
                _availableItems.Push(poolable);
            }
        }

        public TItem Get()
        {
            TItem poolable;

            if (_availableItems.Count > 0)
            {
                poolable = _availableItems.Pop();
            }
            else
            {
                poolable = _factory.Create();
                _allCreatedItems.Add(poolable);
            }
            
            return poolable;
        }

        public void Return(TItem item)
        {
            if (_allCreatedItems.Contains(item) == false)
                throw new ArgumentException("This object from another pool.", nameof(item));
            
            _availableItems.Push(item);
        }

        public void ReturnAll()
        {
            foreach (var createdObject in _allCreatedItems)
            {
                if (!_availableItems.Contains(createdObject))
                    Return(createdObject);
            }
        }
    }
}