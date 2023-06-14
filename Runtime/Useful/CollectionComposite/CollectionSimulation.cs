using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionSimulation : ISimulation
    {
        private readonly IReadOnlyCollection<ISimulation> _collection;

        public CollectionSimulation(IReadOnlyCollection<ISimulation> collection)
        {
            _collection = collection;
        }

        public void StepForward()
        {
            for (int i = 0; i < _collection.Entries.Count; i++)
            {
                _collection.Entries[i].StepForward();
            }
        }
    }
}
