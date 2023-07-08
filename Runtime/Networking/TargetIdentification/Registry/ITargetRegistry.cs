using UPR.Common;

namespace UPR.Networking
{
    public interface ITargetRegistry<TTarget> : IReadOnlyContainer<TTarget>, IReadOnlyTargetRegistry<TTarget>
    {
        void Add(TTarget target, TargetId targetId);
        void Remove(TargetId targetId);
        void Remove(int entryIndex);
    }
}
