using System;
using System.Collections.Generic;

namespace UPR
{
    public class StateHistories : IStateHistory
    {
        private readonly List<IStateHistory> _histories = new List<IStateHistory>();

        public void AddHistory(IStateHistory stateHistory)
        {
            _histories.Add(stateHistory);
        }

        public void SaveStep()
        {
            foreach (var history in _histories)
            {
                history.SaveStep();
            }
        }

        public void Rollback(int steps)
        {
            foreach (var history in _histories)
            {
                history.Rollback(steps);
            }
        }
    }
}
