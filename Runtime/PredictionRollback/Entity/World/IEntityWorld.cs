namespace UPR.PredictionRollback
{
    public interface IEntityWorld<TEntity> : IEntityFinder<TEntity> where TEntity : IEntity
    {
        void RegisterEntity(TEntity entity, EntityId entityId);
        EntityId GetEntityId(TEntity entity);
        bool IsEntityExists(TEntity entity);
    }
}
