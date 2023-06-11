namespace UPR.PredictionRollback
{
    public interface ICommandTargetFinder<out TTarget>
    {
        TTarget GetTarget(TargetId targetId);
        bool IsTargetIdExists(TargetId targetId);
    }
}
