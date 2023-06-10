using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public interface IEntityWorld<TEntity> : IEntityFinder<TEntity> where TEntity : IEntity
    {
        IReadOnlyCollection<TEntity> Entities { get; }
        int CurrentTick { get; }

        void RegisterEntity(TEntity entity, EntityId entityId);
        EntityId GetEntityId(TEntity entity);
        bool IsEntityExists(TEntity entity);
    }
}
