using System.Collections.Generic;
using UPR.Common;

namespace UPR.PredictionRollback
{
    public class CommandTargetRegistry<TTarget> : ICommandTargetRegistry<TTarget>
    {
        private readonly List<TTarget> _targets = new List<TTarget>();
        private readonly Dictionary<TargetId, TTarget> _targetsById = new Dictionary<TargetId, TTarget>();
        private readonly Dictionary<TTarget, TargetId> _idsByTarget = new Dictionary<TTarget, TargetId>();

        public IReadOnlyList<TTarget> Entries => _targets;

        public void Add(TTarget target, TargetId targetId)
        {
            _targets.Add(target);
            _targetsById.Add(targetId, target);
            _idsByTarget.Add(target, targetId);
        }

        public void Remove(TargetId targetId)
        {
            var entity = _targetsById[targetId];
            _targets.RemoveBySwap(entity);
            _idsByTarget.Remove(entity);
            _targetsById.Remove(targetId);
        }

        public TargetId GetTargetId(TTarget target)
        {
            return _idsByTarget[target];
        }

        public bool IsTargetExists(TTarget target)
        {
            return _idsByTarget.ContainsKey(target);
        }

        public bool IsTargetIdExists(TargetId targetId)
        {
            return _targetsById.ContainsKey(targetId);
        }

        public TTarget GetTarget(TargetId targetId)
        {
            return _targetsById[targetId];
        }
    }
}
