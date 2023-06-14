using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionCleanup<TTarget> : IMispredictionCleanup where TTarget : ITickCounter
    {
        private readonly ICollection<TTarget> _collection;

        public CollectionCleanup(ICollection<TTarget> collection)
        {
            _collection = collection;
        }

        public void CleanUp()
        {
            for (int i = _collection.Entries.Count - 1; i >= 0; i--)
            {
                var entry = _collection.Entries[i];
                if (entry.CurrentTick <= 0)
                {
                    _collection.Remove(entry);
                }
            }
        }
    }
}
