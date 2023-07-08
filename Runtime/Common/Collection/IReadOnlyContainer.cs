using System.Collections.Generic;

namespace UPR.Common
{
    public interface IReadOnlyContainer<out TEntry>
    {
        IReadOnlyList<TEntry> Entries { get; }
    }
}
