using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class CommandPlayers : ICommandPlayer
    {
        private readonly List<ICommandPlayer> _commandsPlayers = new List<ICommandPlayer>();

        public void Add(ICommandPlayer commandPlayer)
        {
            if (commandPlayer == null)
                throw new ArgumentNullException(nameof(commandPlayer));

            _commandsPlayers.Add(commandPlayer);
        }

        public void ExecuteCommands(int tick)
        {
            foreach (var commandsPlayer in _commandsPlayers)
            {
                commandsPlayer.ExecuteCommands(tick);
            }
        }
    }
}
