using System;
using System.Collections.Generic;

namespace UPR
{
    public class ReversibleValue<TValue> : IValueHistory<TValue> where TValue : IEquatable<TValue>
    {
        private readonly List<ValueChange> _valueChanges;

        public ReversibleValue(TValue initialValue)
        {
            Value = initialValue;
            _valueChanges = new List<ValueChange>
            {
                new ValueChange(Value, StepsSaved)
            };
        }

        public int StepsSaved { get; private set; }

        public TValue Value { get; set; }

        private TValue LastSaved => _valueChanges[_valueChanges.Count - 1].Value;

        public void SaveStep()
        {
            StepsSaved += 1;

            if (!Value.Equals(LastSaved))
            {
                _valueChanges.Add(new ValueChange(Value, StepsSaved));
            }
        }

        public void Rollback(int steps)
        {
            if (steps > StepsSaved)
            {
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");
            }

            StepsSaved -= steps;

            for (int i = _valueChanges.Count - 1; _valueChanges[i].Step > StepsSaved ; i--)
            {
                _valueChanges.RemoveAt(_valueChanges.Count - 1);
            }

            Value = LastSaved;
        }

        private readonly struct ValueChange
        {
            public ValueChange(TValue value, int step)
            {
                Value = value;
                Step = step;
            }

            public TValue Value { get; }
            public int Step { get; }
        }
    }
}
