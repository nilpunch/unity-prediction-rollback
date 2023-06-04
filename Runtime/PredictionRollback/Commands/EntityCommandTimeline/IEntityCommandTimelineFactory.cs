namespace UPR
{
    public interface IEntityCommandTimelineFactory<TCommand>
    {
        IEntityCommandTimeline<TCommand> CreateForEntity(EntityId entityId);
    }
}
