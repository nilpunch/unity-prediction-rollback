namespace UPR
{
    public interface IWorldCommandTimeline
    {
        int EarliestCommandChange { get; }

        void ApproveChangesUpTo(int tick);

        void ExecuteCommands(int tick);
    }

    public interface IWorldCommandTimeline<TCommand> : IWorldCommandTimeline
    {
        void RemoveAllCommandsDownTo(int tick);
        void RemoveAllCommandsForEntityDownTo(int tick, EntityId entityId);

        void RemoveAllCommandsAt(int tick);
        void RemoveCommandForEntityAt(int tick, EntityId entityId);

        void InsertCommand(int tick, in TCommand command, EntityId entityId);
    }
}
