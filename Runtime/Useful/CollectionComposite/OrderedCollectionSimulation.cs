using System.Collections.Generic;
using UPR.Common;
using UPR.Networking;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class OrderedCollectionSimulation : ISimulation
    {
        private readonly List<ISimulation> _sortedSimulations = new List<ISimulation>();

        private readonly IReadOnlyContainer<ISimulation> _container;
        private readonly IReadOnlyTargetRegistry<ISimulation> _targetRegistry;

        public OrderedCollectionSimulation(IReadOnlyContainer<ISimulation> container, IReadOnlyTargetRegistry<ISimulation> targetRegistry)
        {
            _container = container;
            _targetRegistry = targetRegistry;
        }

        public void StepForward()
        {
            _sortedSimulations.Clear();
            _sortedSimulations.AddRange(_container.Entries);
            _sortedSimulations.Sort((a, b) => _targetRegistry.GetTargetId(a).Value - _targetRegistry.GetTargetId(b).Value);

            foreach (var simulation in _sortedSimulations)
            {
                simulation.StepForward();
            }
        }
    }
}
