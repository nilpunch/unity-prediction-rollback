namespace UPR
{
    public interface ICommandTimeline
    {
        void ExecuteCommands(long tick);
    }

    public interface ICommandTimeline<TCommand> : ICommandTimeline
    {
        void RemoveCommand(long tick, EntityId entityId);
        void InsertCommand(long tick, in TCommand command, EntityId entityId);
    }
}
