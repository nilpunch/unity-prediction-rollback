using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public interface ICollection<out TEntry>
    {
        IReadOnlyList<TEntry> Entries { get; }
    }
}
