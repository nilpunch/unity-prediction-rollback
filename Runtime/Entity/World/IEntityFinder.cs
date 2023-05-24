namespace UPR
{
    public interface IEntityFinder<out TEntity>
    {
        TEntity FindWakeEntity(EntityId entityId);
        EntityStatus GetStatus(EntityId entityId);
    }
}
