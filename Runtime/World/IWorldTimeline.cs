namespace UPR
{
    public interface IWorldTimeline
    {
        void RegisterTimeline<TCommand>(ICommandTimeline<TCommand> commandTimeline);

        void RemoveCommand<TCommand>(int tick, EntityId entityId);
        void InsertCommand<TCommand>(int tick, in TCommand command, EntityId entityId);
        void FastForwardToTick(int targetTick);
    }
}
