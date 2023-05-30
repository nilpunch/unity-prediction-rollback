﻿using System;
using System.Collections.Generic;

namespace UPR.Samples
{
    public class Pool<TItem> : IPool<TItem>
    {
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
            }

            return poolable;
        }

        public void Return(TItem item)
        {
            _availableItems.Push(item);
        }
    }
}
