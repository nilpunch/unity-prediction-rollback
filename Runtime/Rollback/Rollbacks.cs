using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class Rollbacks : IRollback
    {
        private readonly List<IRollback> _rollback;

        public Rollbacks() : this(Enumerable.Empty<IRollback>())
        {
        }

        public Rollbacks(IEnumerable<IRollback> rollbacks)
        {
            _rollback = new List<IRollback>(rollbacks);
        }

        public void AddRollback(IRollback simulation)
        {
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
