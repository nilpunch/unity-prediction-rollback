using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class ReversibleMemoryHistory<TSnapshot> : IReversibleHistory
    {
        private readonly List<TSnapshot> _history;
        private readonly IMemory<TSnapshot> _memory;

        public ReversibleMemoryHistory(IMemory<TSnapshot> memory)
        {
            _memory = memory;
            _history = new List<TSnapshot> { _memory.Save() };
        }

        public int CurrentStep => _history.Count - 1;

        public void SaveStep()
        {
            _history.Add(_memory.Save());
        }

        public void Rollback(int steps)
        {
            if (steps > CurrentStep)
                throw new Exception($"Can't rollback that far. {nameof(CurrentStep)}: {CurrentStep}, Rollbacking: {steps}.");

            if (CurrentStep != 0)
                _history.RemoveRange(_history.Count - steps, steps);

            _memory.Load(_history.Last());
        }
    }
}
