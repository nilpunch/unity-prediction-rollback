using System.Collections.Generic;

namespace UPR.Common
{
    public interface IContainer<TEntry>
    {
        public IList<TEntry> Entries { get; }
    }
}
