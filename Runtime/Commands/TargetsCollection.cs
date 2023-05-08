using System.Collections.Generic;

namespace UPR
{
    public class TargetsCollection<TCommand> : ITargetsCollection<TCommand>
    {
        private Dictionary<EntityId, ICommandTarget<TCommand>> _targets;

        public void AddTarget(EntityId id, ICommandTarget<TCommand> target)
        {
            _targets.Add(id, target);
        }

        public void RemoveTarget(EntityId id)
        {
            _targets.Remove(id);
        }

        public ICommandTarget<TCommand> FindTarget(EntityId id)
        {
            return _targets[id];
        }
    }
}
