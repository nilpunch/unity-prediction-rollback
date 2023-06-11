namespace UPR.PredictionRollback
{
    public class CollectionSimulation : ISimulation
    {
        private readonly ICollection<ISimulation> _collection;

        public CollectionSimulation(ICollection<ISimulation> collection)
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
