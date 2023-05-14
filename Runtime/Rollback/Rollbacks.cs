using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class Rollbacks : IRollback
    {
        private readonly List<IRollback> _rollback = new List<IRollback>();

        public void AddRollback(IRollback simulation)
        {
            if (simulation == null)
                throw new ArgumentNullException(nameof(simulation));

            _rollback.Add(simulation);
        }

        public void Rollback(int steps)
        {
            foreach (var rollback in _rollback)
            {
                rollback.Rollback(steps);
            }
        }
    }
}
