using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class Simulations : ISimulation
    {
        private readonly List<ISimulation> _simulations;

        public Simulations() : this(Enumerable.Empty<ISimulation>())
        {
        }

        public Simulations(IEnumerable<ISimulation> rollbacks)
        {
            _simulations = new List<ISimulation>(rollbacks);
        }

        public void AddSimulation(ISimulation simulation)
        {
            _simulations.Add(simulation);
        }

        public void StepForward()
        {
            foreach (var simulation in _simulations)
            {
                simulation.StepForward();
            }
        }
    }
}
