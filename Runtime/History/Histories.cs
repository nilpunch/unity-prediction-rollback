using System;
using System.Collections.Generic;

namespace UPR
{
    public class Histories : IHistory
    {
        private readonly List<IHistory> _histories = new List<IHistory>();

        public int CurrentStep { get; private set; }

        public void AddHistory(IHistory history)
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
    }
}
