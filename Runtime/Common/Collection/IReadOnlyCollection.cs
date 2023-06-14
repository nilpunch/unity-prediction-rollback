using System.Collections.Generic;

namespace UPR.Common
{
    public interface IReadOnlyCollection<out TEntry>
    {
        IReadOnlyList<TEntry> Entries { get; }
    }
}
