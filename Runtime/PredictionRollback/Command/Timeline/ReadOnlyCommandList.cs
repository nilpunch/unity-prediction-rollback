using System.Collections;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class ReadOnlyCommandList<TCommand> : IReadOnlyList<TCommand>
    {
        private readonly IReadOnlyDictionary<int, TCommand> _timeline;
        private readonly IReadOnlyList<int> _filledTicksInOrder;

        public ReadOnlyCommandList(IReadOnlyDictionary<int, TCommand> timeline, IReadOnlyList<int> filledTicksInOrder)
        {
            _timeline = timeline;
            _filledTicksInOrder = filledTicksInOrder;
        }

        public int Count => _filledTicksInOrder.Count;

        public IEnumerator<TCommand> GetEnumerator()
        {
            foreach (int tick in _filledTicksInOrder)
            {
                yield return _timeline[tick];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TCommand this[int index] => _timeline[_filledTicksInOrder[index]];
    }
}
