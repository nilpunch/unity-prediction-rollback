using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionHistory : IHistory
    {
        private readonly IReadOnlyContainer<IHistory> _container;

        public CollectionHistory(IReadOnlyContainer<IHistory> container)
        {
            _container = container;
        }

        public void SaveStep()
        {
            foreach (var entity in _container.Entries)
            {
                entity.SaveStep();
            }
        }
    }
}
