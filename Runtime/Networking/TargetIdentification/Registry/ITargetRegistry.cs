using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Networking
{
    public interface ITargetRegistry<TTarget> : IReadOnlyCollection<TTarget>, IReadOnlyTargetRegistry<TTarget>
    {
        void Add(TTarget target, TargetId targetId);
        void Remove(TargetId targetId);
        void Remove(int entryIndex);
    }
}
