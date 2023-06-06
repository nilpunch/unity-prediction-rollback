using System;
using System.Collections.Generic;

namespace UPR
{
    public class RarelyChangingValue<TValue> : IHistory, IRollback, IRebase where TValue : IEquatable<TValue>
    {
        private readonly List<ValueChange> _valueChanges;

        public RarelyChangingValue(TValue initialValue)
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

        public void ForgetFromBeginning(int steps)
        {
            if (steps < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steps));
            }

            if (steps > StepsSaved)
            {
                throw new Exception($"Can't forget that far. {nameof(StepsSaved)}: {StepsSaved}, Forgetting: {steps}.");
            }

            StepsSaved -= steps;

            for (int i = 0; i < _valueChanges.Count; i++)
            {
                var valueChange = _valueChanges[i];
                valueChange.Step -= steps;
                _valueChanges[i] = valueChange;
            }

            int elementsToDelete = 0;
            for (int i = 0; i < _valueChanges.Count - 1; i++)
            {
                var current = _valueChanges[i];
                var next = _valueChanges[i + 1];

                if (next.Step <= 0)
                {
                    elementsToDelete += 1;
                }
                else if (next.Step > 0)
                {
                    current.Step = 0;
                    _valueChanges[i] = current;
                    break;
                }
            }

            _valueChanges.RemoveRange(0, elementsToDelete);
        }

        private struct ValueChange
        {
            public ValueChange(TValue value, int step)
            {
                Value = value;
                Step = step;
            }

            public TValue Value { get; }
            public int Step { get; set; }
        }
    }
}
