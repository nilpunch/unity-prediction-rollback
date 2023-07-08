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
            foreach (var commandsPlayer in _container.Entries)
            {
                commandsPlayer.PlayCommands(tick);
            }
        }
    }
}
