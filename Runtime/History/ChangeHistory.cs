using System;
using System.Collections.Generic;

namespace UPR
{
    public class ChangeHistory<TValue> : IReversibleHistory
    {
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

        private readonly Stack<ValueChange> _valueChanges;

        public ChangeHistory(TValue initialValue)
        {
            Value = initialValue;
            _valueChanges = new Stack<ValueChange>();
            _valueChanges.Push(new ValueChange(Value, StepsSaved));
        }

        public int StepsSaved { get; private set; }

        public TValue Value { get; set; }
        public TValue LastSavedValue => _valueChanges.Peek().Value;

        public void SubmitStep()
        {
            StepsSaved += 1;

            if (!EqualityComparer<TValue>.Default.Equals(Value, LastSavedValue))
            {
                _valueChanges.Push(new ValueChange(Value, StepsSaved));
            }
        }

        public void Rollback(int steps)
        {
            if (steps > StepsSaved)
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");

            StepsSaved -= steps;

            while (_valueChanges.Peek().Step > StepsSaved)
            {
                _valueChanges.Pop();
            }

            Value = _valueChanges.Peek().Value;
        }
    }
}
