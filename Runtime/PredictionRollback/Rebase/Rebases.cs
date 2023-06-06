using System;
using System.Collections.Generic;

namespace UPR
{
    public class Rebases : IRebase
    {
        private readonly List<IRebase> _rebases = new List<IRebase>();

        public void Add(IRebase rebase)
        {
            if (rebase == null)
            {
                throw new ArgumentNullException(nameof(rebase));
            }

            _rebases.Add(rebase);
        }

        public void ForgetFromBeginning(int steps)
        {
            foreach (var rebase in _rebases)
            {
                rebase.ForgetFromBeginning(steps);
            }
        }
    }
}
