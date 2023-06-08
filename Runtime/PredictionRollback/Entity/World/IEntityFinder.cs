namespace UPR.PredictionRollback
{
    public interface IEntityFinder<out TEntity>
    {
        TEntity GetExistingEntity(EntityId entityId);
        bool IsEntityIdExists(EntityId entityId);
    }
}
