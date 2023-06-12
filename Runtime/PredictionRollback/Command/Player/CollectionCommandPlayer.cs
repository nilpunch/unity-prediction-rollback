using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class CollectionCommandPlayer : ICommandPlayer
    {
        private readonly ICollection<ICommandPlayer> _collection;

        public CollectionCommandPlayer(ICollection<ICommandPlayer> collection)
        {
            _collection = collection;
        }

        public void ExecuteCommands(int tick)
        {
            foreach (var commandsPlayer in _collection.Entries)
            {
                commandsPlayer.ExecuteCommands(tick);
            }
        }
    }
}
