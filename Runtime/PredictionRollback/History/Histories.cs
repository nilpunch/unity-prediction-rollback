using System;
using System.Collections.Generic;

namespace UPR
{
    public class Histories : IHistory
    {
        private readonly List<IHistory> _histories = new List<IHistory>();

        public void Add(IHistory history)
        {
            if (history == null)
            {
                throw new ArgumentNullException(nameof(history));
            }

            _histories.Add(history);
        }

        public void SaveStep()
        {
            foreach (var history in _histories)
            {
                history.SaveStep();
            }
        }
    }
}
