namespace UPR
{
    public interface ICommandTimelineFactory<TCommand>
    {
        ICommandTimeline<TCommand> CreateForEntity(EntityId entityId);
    }
}
