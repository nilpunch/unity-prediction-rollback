using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionCommandPlayer : ICommandPlayer
    {
        private readonly IReadOnlyContainer<ICommandPlayer> _container;

        public CollectionCommandPlayer(IReadOnlyContainer<ICommandPlayer> container)
        {
            _container = container;
        }

        public void PlayCommands(int tick)
        {
            for (int index = 0; index < _container.Entries.Count; index++)
            {
                _container.Entries[index].PlayCommands(tick);
            }
        }
    }
}
