using UPR.Networking;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class TargetRegistryCleanup<TTarget> : IMispredictionCleanup where TTarget : ITickCounter
    {
        private readonly ITargetRegistry<TTarget> _targetRegistry;

        public TargetRegistryCleanup(ITargetRegistry<TTarget> targetRegistry)
        {
            _targetRegistry = targetRegistry;
        }

        public void CleanUp()
        {
            for (int entryIndex = _targetRegistry.Entries.Count - 1; entryIndex >= 0; entryIndex--)
            {
                var entry = _targetRegistry.Entries[entryIndex];
                if (entry.CurrentTick <= 0)
                {
                    _targetRegistry.Remove(entryIndex);
                }
            }
        }
    }
}
