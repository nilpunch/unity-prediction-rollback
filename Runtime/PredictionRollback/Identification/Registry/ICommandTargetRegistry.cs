namespace UPR.PredictionRollback
{
    public interface ICommandTargetRegistry<TTarget> : ICollection<TTarget>, ICommandTargetFinder<TTarget>
    {
        void Add(TTarget target, TargetId targetId);
        void Remove(TargetId targetId);
        TargetId GetTargetId(TTarget target);
        bool IsTargetExists(TTarget target);
    }
}
