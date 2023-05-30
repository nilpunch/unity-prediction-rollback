using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class ChangeHistory<TValue> : IHistory, IRollback
    {
        private readonly List<ValueChange> _valueChanges;

        public ChangeHistory(TValue initialValue)
        {
            Value = initialValue;
            _valueChanges = new List<ValueChange>();
            _valueChanges.Add(new ValueChange(Value, StepsSaved));
        }

        public int StepsSaved { get; private set; }

        public TValue Value { get; set; }

        public void SaveStep()
        {
            StepsSaved += 1;

            if (!EqualityComparer<TValue>.Default.Equals(Value, _valueChanges.Last().Value))
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

            Value = _valueChanges.Last().Value;
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
