using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class Simulations : ISimulation
    {
        private readonly List<ISimulation> _simulations = new List<ISimulation>();

        public void Add(ISimulation simulation)
        {
            if (simulation == null)
                throw new ArgumentNullException(nameof(simulation));

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
