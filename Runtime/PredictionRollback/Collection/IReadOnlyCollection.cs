using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public interface IReadOnlyCollection<out TEntry>
    {
        IReadOnlyList<TEntry> Entries { get; }
    }
}
