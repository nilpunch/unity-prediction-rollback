using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class MemoryHistory<TSnapshot> : IReversibleHistory
    {
        private readonly List<TSnapshot> _history;
        private readonly IMemory<TSnapshot> _memory;

        public MemoryHistory(IMemory<TSnapshot> memory)
        {
            _memory = memory;
            _history = new List<TSnapshot> { _memory.Save() };
        }

        public int StepsSaved => _history.Count - 1;

        public void SaveStep()
        {
            _history.Add(_memory.Save());
        }

        public void Rollback(int steps)
        {
            if (steps > StepsSaved)
            {
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");
            }

            if (StepsSaved != 0)
            {
                _history.RemoveRange(_history.Count - steps, steps);
            }

            _memory.Load(_history.Last());
        }
    }
}
