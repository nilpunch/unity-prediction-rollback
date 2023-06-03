using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class ValueHistory<TValue> : IValueHistory<TValue>
    {
        private readonly List<TValue> _history;

        public ValueHistory(TValue initial)
        {
            _history = new List<TValue> { initial };
        }

        public int StepsSaved => _history.Count - 1;

        public TValue Value { get; set; }

        public void SaveStep()
        {
            _history.Add(Value);
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

            Value = _history.Last();
        }
    }
}
