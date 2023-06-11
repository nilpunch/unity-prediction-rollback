namespace UPR.PredictionRollback
{
    public class CollectionHistory : IHistory
    {
        private readonly ICollection<IHistory> _collection;

        public CollectionHistory(ICollection<IHistory> collection)
        {
            _collection = collection;
        }

        public void SaveStep()
        {
            foreach (var entity in _collection.Entries)
            {
                entity.SaveStep();
            }
        }
    }
}
