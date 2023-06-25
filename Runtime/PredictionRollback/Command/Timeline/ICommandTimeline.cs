using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public interface ICommandTimeline<TCommand>
    {
        IReadOnlyList<TCommand> SolidUnpredictedCommands { get; }

        void RemoveAllCommandsInRange(int fromTickInclusive, int toTickInclusive);
        void RemoveAllCommandsDownTo(int tick);
        void RemoveCommand(int tick);
        void InsertCommand(int tick, in TCommand command);
        int GetLatestTickWithSolidCommandBefore(int tickInclusive);
        bool HasCommand(int tick);
        bool HasSameCommand(int tick, TCommand command);
        TCommand GetCommand(int tick);
    }
}
