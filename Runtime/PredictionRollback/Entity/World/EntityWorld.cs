using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, IMispredictionCleanup where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesById = new Dictionary<EntityId, TEntity>();
        private readonly Dictionary<TEntity, EntityId> _idsByEntity = new Dictionary<TEntity, EntityId>();

        public IReadOnlyList<TEntity> Entities => _entities;

        public void RegisterEntity(TEntity entity, EntityId entityId)
        {
            _entities.Add(entity);
            _entitiesById.Add(entityId, entity);
            _idsByEntity.Add(entity, entityId);
        }

        public EntityId GetEntityId(TEntity entity)
        {
            return _idsByEntity[entity];
        }

        public bool IsEntityExists(TEntity entity)
        {
            return _idsByEntity.ContainsKey(entity);
        }

        public bool IsEntityIdExists(EntityId entityId)
        {
            return _entitiesById.ContainsKey(entityId);
        }

        public TEntity GetExistingEntity(EntityId entityId)
        {
            return _entitiesById[entityId];
        }

        public void Cleanup()
        {
            RemoveNotBornEntities();
        }

        private void RemoveNotBornEntities()
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                var entity = _entities[i];
                if (entity.SavedSteps <= 0)
                {
                    _entities.RemoveAt(i);
                    _entitiesById.Remove(_idsByEntity[entity]);
                    _idsByEntity.Remove(entity);
                }
            }
        }
    }
}
