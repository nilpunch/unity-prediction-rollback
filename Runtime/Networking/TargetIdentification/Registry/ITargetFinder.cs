namespace UPR.Networking
{
    public interface ITargetFinder<out TTarget>
    {
        TTarget GetTarget(TargetId targetId);
        bool IsTargetIdExists(TargetId targetId);
    }
}
