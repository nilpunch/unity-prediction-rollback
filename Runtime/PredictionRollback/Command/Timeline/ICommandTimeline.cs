﻿namespace UPR.PredictionRollback
{
    public interface IReadOnlyCommandTimeline<TCommand>
    {
        int GetLatestTickWithCommandInclusiveBefore(int tick);
        bool HasCommand(int tick);
        TCommand GetCommand(int tick);
    }

    public interface ICommandTimeline<TCommand> : IReadOnlyCommandTimeline<TCommand>
    {
        void RemoveAllCommandsDownTo(int tick);
        void RemoveCommand(int tick);
        void InsertCommand(int tick, in TCommand command);
    }
}
