using System;
using System.Collections.Generic;

namespace UPR
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, IHistory, ISimulation, IRollback where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesById = new Dictionary<EntityId, TEntity>();
        private readonly Dictionary<TEntity, EntityId> _idsByEntity = new Dictionary<TEntity, EntityId>();

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

        public bool IsAlive(EntityId entityId)
        {
            if (_entitiesById.ContainsKey(entityId))
            {
                return _entitiesById[entityId].IsAlive;
            }

            return false;
        }

        public TEntity FindAliveEntity(EntityId entityId)
        {
            if (_entitiesById.TryGetValue(entityId, out var entity) && entity.IsAlive)
            {
                return entity;
            }

            return default;
        }


        public void StepForward()
        {
            // Using for loop to be able to register new entities during simulation
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].StepForward();
            }
        }

        public void SaveStep()
        {
            foreach (var entity in _entities)
            {
                entity.SaveStep();
            }
        }

        public void Rollback(int steps)
        {
            foreach (var entity in _entities)
            {
                entity.Rollback(steps);
            }

            LoseTrackOfVolatileEntities();
        }

        private void LoseTrackOfVolatileEntities()
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                var entity = _entities[i];
                if (entity.Age <= 0)
                {
                    _entities.RemoveAt(i);
                    _entitiesById.Remove(_idsByEntity[entity]);
                    _idsByEntity.Remove(entity);
                }
            }
        }
    }
}
