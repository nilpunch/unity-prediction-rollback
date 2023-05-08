using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class StateHistory<TSnapshot> : IStateHistory
    {
        private readonly List<TSnapshot> _history;
        private readonly IMemory<TSnapshot> _memory;

        public StateHistory(IMemory<TSnapshot> memory)
        {
            _memory = memory;
            _history = new List<TSnapshot> {_memory.Save()};
        }

        public int HistoryLength => _history.Count - 1;

        public void SaveStep()
        {
            _history.Add(_memory.Save());
        }

        public void Rollback(int ticks)
        {
            if (ticks > HistoryLength)
                throw new Exception($"Can't rollback that far. {nameof(HistoryLength)}: {HistoryLength}, Rollbacking: {ticks}.");

            if (HistoryLength != 0)
                _history.RemoveRange(_history.Count - 1 - ticks, ticks);

            _memory.Load(_history.Last());
        }
    }
}
