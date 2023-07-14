using System.Collections.Generic;
using UPR.Common;

namespace UPR.Networking
{
    public class CommandTimelineRegistry<TTarget> : ICommandTimelineRegistry<TTarget>, ICommandTimelineFinder<TTarget>
    {
        private readonly List<TTarget> _targets = new List<TTarget>();
        private readonly Dictionary<CommandTimelineId, TTarget> _targetsById = new Dictionary<CommandTimelineId, TTarget>();
        private readonly Dictionary<TTarget, CommandTimelineId> _idsByTarget = new Dictionary<TTarget, CommandTimelineId>();

        public void Add(TTarget target, CommandTimelineId commandTimelineId)
        {
            _targets.Add(target);
            _targetsById.Add(commandTimelineId, target);
            _idsByTarget.Add(target, commandTimelineId);
        }

        public void Remove(CommandTimelineId commandTimelineId)
        {
            var target = _targetsById[commandTimelineId];
            _targets.RemoveBySwap(target);
            _targetsById.Remove(commandTimelineId);
            _idsByTarget.Remove(target);
        }

        public void Remove(int entryIndex)
        {
            var target = _targets[entryIndex];
            _targets.RemoveBySwap(entryIndex);
            _targetsById.Remove(_idsByTarget[target]);
            _idsByTarget.Remove(target);
        }

        public CommandTimelineId GetTargetId(TTarget target)
        {
            return _idsByTarget[target];
        }

        public bool IsTargetExists(TTarget target)
        {
            return _idsByTarget.ContainsKey(target);
        }

        public bool IsCommandTimelineExists(CommandTimelineId commandTimelineId)
        {
            return _targetsById.ContainsKey(commandTimelineId);
        }

        public TTarget GetCommandTimeline(CommandTimelineId commandTimelineId)
        {
            return _targetsById[commandTimelineId];
        }
    }
}
