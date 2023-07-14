using System.Collections.Generic;

namespace UPR.Common
{
    public class Container<TEntry> : IContainer<TEntry>
    {
        private readonly List<TEntry> _entries;

        public Container(int capacity = 32)
        {
            _entries = new List<TEntry>(capacity);
        }

        IReadOnlyList<TEntry> IReadOnlyContainer<TEntry>.Entries => _entries;

        public IList<TEntry> Entries => _entries;
    }
}
