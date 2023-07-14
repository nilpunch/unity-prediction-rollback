using System;
using System.Collections.Generic;

namespace UPR.Useful
{
    public class SavedTicks
    {
        private readonly List<int> _ticks = new List<int>();

        public bool IsEmpty => _ticks.Count == 0;

        public void Save(int tick)
        {
            if (_ticks.Count != 0 && _ticks[_ticks.Count - 1] >= tick)
                throw new ArgumentOutOfRangeException(nameof(tick), "Ticks must be saved in order.");

            _ticks.Add(tick);
        }

        public int AmountBefore(int tickInclusive)
        {
            int index = _ticks.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index;
            }

            return _ticks.Count - index;
        }

        public int AmountAfter(int tickInclusive)
        {
            int index = _ticks.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index;
            }

            return index + 1;
        }

        public void RemoveAfter(int tickInclusive)
        {
            int index = _ticks.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index - 1;
            }

            if (index >= 0 && index != _ticks.Count - 1)
            {
                _ticks.RemoveRange(index + 1, _ticks.Count - (index + 1));
            }
        }

        public void RemoveBefore(int tickInclusive)
        {
            int index = _ticks.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index;
            }

            _ticks.RemoveRange(0, index + 1);
        }

        public void Clear()
        {
            _ticks.Clear();
        }
    }
}
