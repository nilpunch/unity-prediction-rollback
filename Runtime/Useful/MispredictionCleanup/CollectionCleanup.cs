using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionCleanup<TTarget> : IMispredictionCleanup where TTarget : ITickCounter
    {
        private readonly IContainer<TTarget> _container;

        public CollectionCleanup(IContainer<TTarget> container)
        {
            _container = container;
        }

        public void CleanUp()
        {
            for (int entryIndex = _container.Entries.Count - 1; entryIndex >= 0; entryIndex--)
            {
                var entry = _container.Entries[entryIndex];
                if (entry.CurrentTick <= 0)
                {
                    _container.Entries.RemoveAt(entryIndex);
                }
            }
        }
    }
}
