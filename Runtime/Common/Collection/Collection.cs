﻿using System.Collections.Generic;

namespace UPR.Common
{
    public class Collection<TEntry> : ICollection<TEntry>
    {
        private readonly List<TEntry> _entries = new List<TEntry>();

        public Collection(int capacity = 32)
        {
            _entries = new List<TEntry>(capacity);
        }

        public IReadOnlyList<TEntry> Entries => _entries;

        public void Add(TEntry entry)
        {
            _entries.Add(entry);
        }

        public void Remove(TEntry entry)
        {
            _entries.Remove(entry);
        }
    }
}
