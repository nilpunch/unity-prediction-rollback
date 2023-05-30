namespace UPR
{
    public interface IEntityFinder<out TEntity>
    {
        TEntity GetExistingEntity(EntityId entityId);
        bool IsExists(EntityId entityId);
    }
}
