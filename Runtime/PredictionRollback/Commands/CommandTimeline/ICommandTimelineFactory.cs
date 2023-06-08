namespace UPR.PredictionRollback
{
    public interface ICommandTimelineFactory<TCommand>
    {
        ICommandTimeline<TCommand> CreateForEntity(EntityId entityId);
    }
}
