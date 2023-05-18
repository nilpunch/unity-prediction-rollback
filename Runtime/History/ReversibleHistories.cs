﻿using System;
using System.Collections.Generic;

namespace UPR
{
    public class ReversibleHistories : IReversibleHistory
    {
        private readonly List<IReversibleHistory> _histories = new List<IReversibleHistory>();

        public int StepsSaved { get; private set; }

        public void AddHistory(IReversibleHistory history)
        {
            if (history.StepsSaved != StepsSaved)
                throw new Exception($"Can't add history: {nameof(StepsSaved)}'s are not synchronised. Current: " + StepsSaved + ", Added: " + history.StepsSaved);

            _histories.Add(history);
        }

        public void SaveStep()
        {
            foreach (var history in _histories)
            {
                history.SaveStep();
            }

            StepsSaved += 1;
        }

        public void Rollback(int steps)
        {
            foreach (var history in _histories)
            {
                history.Rollback(steps);
            }

            StepsSaved -= steps;
        }
    }
}
