using System;
using System.Collections.Generic;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class FrequentlyChangingValue<TValue> : IHistory, IRollback, IRebase where TValue : IEquatable<TValue>
    {
        private readonly List<TValue> _history;

        public FrequentlyChangingValue(TValue initialValue)
        {
            Value = initialValue;
            _history = new List<TValue> { Value };
        }

        public int StepsSaved => _history.Count - 1;

        public TValue Value { get; set; }

        private TValue LastSaved => _history[_history.Count - 1];

        public void SaveStep()
        {
            _history.Add(Value);
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (steps > StepsSaved)
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");

            if (StepsSaved != 0)
            {
                _history.RemoveRange(_history.Count - steps, steps);
            }

            Value = LastSaved;
        }

        public void ForgetFromBeginning(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (steps > StepsSaved)
                throw new Exception($"Can't forget that far. {nameof(StepsSaved)}: {StepsSaved}, Forgetting: {steps}.");

            _history.RemoveRange(0, steps);
        }
    }
}
