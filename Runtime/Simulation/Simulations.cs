using System.Collections.Generic;

namespace UPR
{
    public class Simulations : ISimulation
    {
        private readonly List<ISimulation> _simulations = new List<ISimulation>();

        public void AddSimulation(ISimulation simulation)
        {
            _simulations.Add(simulation);
        }

        public void StepForward(int currentTick)
        {
            foreach (var simulation in _simulations)
            {
                simulation.StepForward(currentTick);
            }
        }
    }
}
