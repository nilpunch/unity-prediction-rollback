using System.Collections.Generic;

namespace UPR
{
    public class Rollbacks : IRollback
    {
        private readonly List<IRollback> _rollback = new List<IRollback>();

        public void AddRollback(IRollback simulation)
        {
            _rollback.Add(simulation);
        }

        public void Rollback(int steps)
        {
            foreach (var simulation in _rollback)
            {
                simulation.Rollback(steps);
            }
        }
    }
}
