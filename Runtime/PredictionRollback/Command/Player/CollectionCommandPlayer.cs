namespace UPR.PredictionRollback
{
    public class CollectionCommandPlayer : ICommandPlayer
    {
        private readonly IReadOnlyCollection<ICommandPlayer> _collection;

        public CollectionCommandPlayer(IReadOnlyCollection<ICommandPlayer> collection)
        {
            _collection = collection;
        }

        public void PlayCommands(int tick)
        {
            foreach (var commandsPlayer in _collection.Entries)
            {
                commandsPlayer.PlayCommands(tick);
            }
        }
    }
}
