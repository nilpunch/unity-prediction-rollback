using System;
using System.Collections.Generic;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class TickStamps
    {
        private readonly List<int> _timeStamps = new List<int>();

        public void Stamp(int tick)
        {
            if (_timeStamps.Count != 0 && _timeStamps[_timeStamps.Count - 1] >= tick)
                throw new ArgumentOutOfRangeException(nameof(tick));

            _timeStamps.Add(tick);
        }

        public int StampsAmountUntil(int tickInclusive)
        {
            int index = _timeStamps.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index;
            }

            return _timeStamps.Count - index;
        }

        public void RemoveStampsUntil(int tickInclusive)
        {
            int index = _timeStamps.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index - 1;
            }

            if (index >= 0 && index != _timeStamps.Count - 1)
            {
                _timeStamps.RemoveRange(index + 1, _timeStamps.Count - (index + 1));
            }
        }

        public void ForgetUntil(int tickInclusive)
        {
            int index = _timeStamps.BinarySearch(tickInclusive);

            if (index < 0)
            {
                index = ~index - 1;
            }

            if (index >= 0)
            {
                _timeStamps.RemoveRange(0, index);
            }
        }
    }
}
