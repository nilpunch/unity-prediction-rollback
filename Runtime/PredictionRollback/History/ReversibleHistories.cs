using System;
using System.Collections.Generic;

namespace UPR
{
    public class ReversibleHistories : IHistory, IRollback
    {
        private readonly List<ReversibleHistory> _reversibleHistories = new List<ReversibleHistory>();

        public int StepsSaved { get; private set; }

        public void AddReversibleHistory<TReversibleHistory>(TReversibleHistory reversibleHistory) where TReversibleHistory : IHistory, IRollback
        {
            _reversibleHistories.Add(new ReversibleHistory(reversibleHistory, reversibleHistory));
        }

        public void SaveStep()
        {
            foreach (var reversibleHistory in _reversibleHistories)
            {
                reversibleHistory.History.SaveStep();
            }

            StepsSaved += 1;
        }

        public void Rollback(int steps)
        {
            foreach (var reversibleHistory in _reversibleHistories)
            {
                reversibleHistory.Rollback.Rollback(steps);
            }

            StepsSaved -= steps;
        }

        private struct ReversibleHistory
        {
            public ReversibleHistory(IHistory history, IRollback rollback)
            {
                History = history;
                Rollback = rollback;
            }

            public IHistory History { get; }
            public IRollback Rollback { get; }
        }
    }
}
