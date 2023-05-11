using System;
using System.Collections.Generic;

namespace UPR
{
    public class ReversibleHistories : IReversibleHistory
    {
        private readonly List<IReversibleHistory> _histories = new List<IReversibleHistory>();

        public int CurrentStep { get; private set; }

        public void AddHistory(IReversibleHistory history)
        {
            if (history.CurrentStep != CurrentStep)
                throw new Exception($"Can't add history: {nameof(CurrentStep)}'s are not synchronised.");

            _histories.Add(history);
        }

        public void SaveStep()
        {
            foreach (var history in _histories)
            {
                history.SaveStep();
            }

            CurrentStep += 1;
        }

        public void Rollback(int steps)
        {
            foreach (var history in _histories)
            {
                history.Rollback(steps);
            }

            CurrentStep -= steps;
        }
    }
}
