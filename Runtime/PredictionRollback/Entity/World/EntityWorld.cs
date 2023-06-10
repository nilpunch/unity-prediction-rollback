using System.Collections.Generic;
using UPR.Common;

namespace UPR.PredictionRollback
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>
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

        public void DeregisterEntity(EntityId entityId)
        {
            var entity = _entitiesById[entityId];
            _entities.RemoveBySwap(entity);
            _idsByEntity.Remove(entity);
            _entitiesById.Remove(entityId);
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
    }
}
