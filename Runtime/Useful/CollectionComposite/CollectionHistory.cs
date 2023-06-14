using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionHistory : IHistory
    {
        private readonly IReadOnlyCollection<IHistory> _collection;

        public CollectionHistory(IReadOnlyCollection<IHistory> collection)
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
