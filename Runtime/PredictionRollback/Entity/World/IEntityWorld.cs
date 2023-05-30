namespace UPR
{
    public interface IEntityWorld<TEntity> : IEntityFinder<TEntity> where TEntity : IEntity
    {
        void RegisterEntity(TEntity entity, EntityId entityId);
        EntityId GetEntityId(TEntity entity);
    }
}