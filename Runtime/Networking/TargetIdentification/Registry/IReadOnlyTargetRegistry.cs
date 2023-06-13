namespace UPR.Networking
{
    public interface IReadOnlyTargetRegistry<in TTarget>
    {
        TargetId GetTargetId(TTarget target);
        bool IsTargetExists(TTarget target);
    }
}
