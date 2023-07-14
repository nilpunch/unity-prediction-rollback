using System.Collections.Generic;

namespace UPR.Common
{
    public interface IContainer<TEntry> : IReadOnlyContainer<TEntry>
    {
        public new IList<TEntry> Entries { get; }
    }
}
