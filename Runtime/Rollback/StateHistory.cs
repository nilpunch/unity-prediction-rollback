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
            _history = new List<TSnapshot>();
        }

        public Checksum Checksum => new Checksum(_history.Last().GetHashCode());

        public void SaveStep()
        {
            _history.Add(_memory.Save());
        }

        public void Rollback(int steps)
        {
            _history.RemoveRange(_history.Count - 1 - steps, steps);
            _memory.Load(_history.Last());
        }
    }
}
