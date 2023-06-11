namespace UPR.PredictionRollback
{
    public interface ICommandTimelineFactory<TCommand>
    {
        ICommandTimeline<TCommand> CreateForEntity(TargetId targetId);
    }
}
