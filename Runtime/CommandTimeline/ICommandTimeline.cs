using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public interface ICommandTimeline<TCommand> : IReadOnlyCommandTimeline<TCommand>
    {
        IReadOnlyList<TCommand> FilledCommands { get; }

        void RemoveAllCommandsInRange(int fromTickInclusive, int toTickInclusive);
        void RemoveAllCommandsDownTo(int tickExclusive);
        void RemoveCommand(int tick);
        void InsertCommand(int tick, in TCommand command);
    }
}
