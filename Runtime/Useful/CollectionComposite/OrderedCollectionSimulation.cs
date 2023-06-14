﻿using System.Collections.Generic;
using UPR.Networking;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class OrderedCollectionSimulation : ISimulation
    {
        private readonly List<ISimulation> _sortedSimulations = new List<ISimulation>();

        private readonly Common.IReadOnlyCollection<ISimulation> _collection;
        private readonly IReadOnlyTargetRegistry<ISimulation> _targetRegistry;

        public OrderedCollectionSimulation(Common.IReadOnlyCollection<ISimulation> collection, IReadOnlyTargetRegistry<ISimulation> targetRegistry)
        {
            _collection = collection;
            _targetRegistry = targetRegistry;
        }

        public void StepForward()
        {
            _sortedSimulations.Clear();
            _sortedSimulations.AddRange(_collection.Entries);
            _sortedSimulations.Sort((a, b) => _targetRegistry.GetTargetId(a).Value - _targetRegistry.GetTargetId(b).Value);

            foreach (var simulation in _sortedSimulations)
            {
                simulation.StepForward();
            }
        }
    }
}