namespace UPR
{
    public interface ICommandTimeline
    {
        void ExecuteCommands(int tick);
    }

    public interface ICommandTimeline<TCommand> : ICommandTimeline
    {
        void RemoveCommand(int tick, EntityId entityId);
        void InsertCommand(int tick, in TCommand command, EntityId entityId);
    }
}
