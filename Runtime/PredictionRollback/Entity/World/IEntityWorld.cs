namespace UPR.PredictionRollback
{
    public interface IEntityWorld<TEntity> : IEntityCollection<TEntity>, IEntityFinder<TEntity>
    {
        void RegisterEntity(TEntity entity, EntityId entityId);
        void DeregisterEntity(EntityId entityId);
        EntityId GetEntityId(TEntity entity);
        bool IsEntityExists(TEntity entity);
    }
}
