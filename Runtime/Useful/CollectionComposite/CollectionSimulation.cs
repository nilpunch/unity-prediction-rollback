using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionSimulation : ISimulation
    {
        private readonly IReadOnlyContainer<ISimulation> _container;

        public CollectionSimulation(IReadOnlyContainer<ISimulation> container)
        {
            _container = container;
        }

        public void StepForward()
        {
            for (int i = 0; i < _container.Entries.Count; i++)
            {
                _container.Entries[i].StepForward();
            }
        }
    }
}
