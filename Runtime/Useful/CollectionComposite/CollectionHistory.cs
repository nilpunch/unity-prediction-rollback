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
            for (int index = 0; index < _container.Entries.Count; index++)
            {
                _container.Entries[index].SaveStep();
            }
        }
    }
}
