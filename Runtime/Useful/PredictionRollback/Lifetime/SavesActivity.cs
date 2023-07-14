using System.Collections.Generic;

namespace UPR.Useful
{
    public class SavesActivity<TEntry>
    {
        private readonly Dictionary<TEntry, SavedTicks> _entryTicks = new Dictionary<TEntry, SavedTicks>();
        private readonly IPool<SavedTicks> _pool = new Pool<SavedTicks>(Factory<SavedTicks>.Default);

        public void MarkSave(TEntry entry, int tick)
        {
            if (!_entryTicks.TryGetValue(entry, out var ticks))
            {
                ticks = _pool.Get();
                _entryTicks.Add(entry, ticks);
            }

            ticks.Save(tick);
        }

        public int CanRollbackAmount(TEntry entry, int rollbackTick)
        {
            if (_entryTicks.TryGetValue(entry, out var ticks))
            {
                return ticks.AmountAfter(rollbackTick);
            }

            return 0;
        }

        public int CanForgetAmount(TEntry entry, int forgetTick)
        {
            if (_entryTicks.TryGetValue(entry, out var ticks))
            {
                return ticks.AmountBefore(forgetTick);
            }

            return 0;
        }

        public void RemoveTicksAfter(int tickInclusive)
        {
            foreach (var entryTicks in _entryTicks)
            {
                entryTicks.Value.RemoveAfter(tickInclusive);
                if (entryTicks.Value.IsEmpty)
                {
                    Cache.EntriesToDelete.Add(entryTicks.Key);
                }
            }

            foreach (var entry in Cache.EntriesToDelete)
            {
                _pool.Return(_entryTicks[entry]);
                _entryTicks.Remove(entry);
            }

            Cache.EntriesToDelete.Clear();
        }

        public void RemoveTicksBefore(int tickInclusive)
        {
            foreach (var entryTicks in _entryTicks)
            {
                entryTicks.Value.RemoveBefore(tickInclusive);
                if (entryTicks.Value.IsEmpty)
                {
                    Cache.EntriesToDelete.Add(entryTicks.Key);
                }
            }

            foreach (var entry in Cache.EntriesToDelete)
            {
                _pool.Return(_entryTicks[entry]);
                _entryTicks.Remove(entry);
            }

            Cache.EntriesToDelete.Clear();
        }

        private static class Cache
        {
            public static HashSet<TEntry> EntriesToDelete { get; } = new HashSet<TEntry>();
        }
    }
}
