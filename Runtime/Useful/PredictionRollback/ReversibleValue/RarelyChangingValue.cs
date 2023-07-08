using System;
using System.Collections.Generic;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class RarelyChangingValue<TValue> : IHistory, IRollback, IRebase
    {
        private readonly IEqualityComparer<TValue> _equalityComparer;
        private readonly List<ValueChange> _valueChangeChanges;

        public RarelyChangingValue(TValue initialValue, IEqualityComparer<TValue> equalityComparer = null)
        {
            _equalityComparer = equalityComparer ?? EqualityComparer<TValue>.Default;
            Value = initialValue;
            _valueChangeChanges = new List<ValueChange> { new ValueChange(Value, 0) };
        }

        public int StepsSaved { get; private set; }

        public TValue Value { get; set; }

        public TValue LastSaved => _valueChangeChanges[_valueChangeChanges.Count - 1].Value;
        public TValue FirstSaved => _valueChangeChanges[0].Value;
        public IReadOnlyList<ValueChange> SavedValueChanges => _valueChangeChanges;

        public void SaveStep()
        {
            StepsSaved += 1;

            if (!Value.Equals(LastSaved))
            {
                _valueChangeChanges.Add(new ValueChange(Value, StepsSaved));
            }
        }

        public void Rollback(int steps)
        {
            if (steps > StepsSaved)
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");

            StepsSaved -= steps;

            int index = _valueChangeChanges.BinarySearch(new ValueChange(default, StepsSaved));

            if (index < 0)
            {
                index = ~index - 1;
            }

            if (index != _valueChangeChanges.Count - 1 && index >= 0)
            {
                _valueChangeChanges.RemoveRange(index + 1, _valueChangeChanges.Count - index - 1);
            }

            Value = LastSaved;
        }

        public void ForgetFromBeginning(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (steps > StepsSaved)
                throw new Exception($"Can't forget that far. {nameof(StepsSaved)}: {StepsSaved}, Forgetting: {steps}.");

            StepsSaved -= steps;

            for (int i = 0; i < _valueChangeChanges.Count; i++)
            {
                var valueChange = _valueChangeChanges[i];
                valueChange.Tick -= steps;
                _valueChangeChanges[i] = valueChange;
            }

            int elementsToDelete = 0;
            for (int i = 0; i < _valueChangeChanges.Count - 1; i++)
            {
                var current = _valueChangeChanges[i];
                var next = _valueChangeChanges[i + 1];

                if (next.Tick <= 0)
                {
                    elementsToDelete += 1;
                }
                else if (next.Tick > 0)
                {
                    current.Tick = 0;
                    _valueChangeChanges[i] = current;
                    break;
                }
            }

            _valueChangeChanges.RemoveRange(0, elementsToDelete);
        }

        public struct ValueChange : IComparable<ValueChange>
        {
            public ValueChange(TValue value, int tick)
            {
                Value = value;
                Tick = tick;
            }

            public TValue Value { get; }
            public int Tick { get; set; }

            public int CompareTo(ValueChange other)
            {
                return Tick.CompareTo(other.Tick);
            }
        }
    }
}
