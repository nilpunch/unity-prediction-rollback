namespace UPR
{
    public interface IEntityFinder<out TEntity>
    {
        TEntity FindAliveEntity(EntityId entityId);
        bool IsAlive(EntityId entityId);
    }
}
