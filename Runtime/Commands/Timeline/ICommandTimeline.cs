﻿namespace UPR
{
    public interface ICommandTimeline
    {
        int EarliestCommandChange { get; }

        void ApproveChangesUpTo(int tick);

        void ExecuteCommands(int tick);
    }

    public interface ICommandTimeline<TCommand> : ICommandTimeline
    {
        void RemoveCommand(int tick, EntityId entityId);
        void InsertCommand(int tick, in TCommand command, EntityId entityId);
    }
}
