using System;
using System.Collections.Generic;

namespace UPR
{
    public class StateHistories : IStateHistory
    {
        private readonly List<IStateHistory> _histories = new List<IStateHistory>();

        public int HistoryLength { get; private set; }

        public void AddHistory(IStateHistory stateHistory)
        {
            if (stateHistory.HistoryLength != HistoryLength)
                throw new Exception($"Can't add history: {nameof(HistoryLength)}'s are not synchronised.");

            _histories.Add(stateHistory);
        }

        public void SaveStep()
        {
            foreach (var history in _histories)
            {
                history.SaveStep();
            }

            HistoryLength += 1;
        }

        public void Rollback(int ticks)
        {
            foreach (var history in _histories)
            {
                history.Rollback(ticks);
            }

            HistoryLength -= ticks;
        }
    }
}
