namespace UPR
{
    public interface ITargetsCollection<TCommand>
    {
        void AddTarget(EntityId id, ICommandTarget<TCommand> target);
        void RemoveTarget(EntityId id);
        ICommandTarget<TCommand> FindTarget(EntityId id);
    }
}
