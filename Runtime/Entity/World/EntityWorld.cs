using System;
using System.Collections.Generic;

namespace UPR
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, IHistory, ISimulation, IRollback where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesById = new Dictionary<EntityId, TEntity>();
        private readonly Dictionary<TEntity, EntityId> _idsByEntity = new Dictionary<TEntity, EntityId>();

        private int CurrentStep { get; set; }

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

        public bool IsExists(EntityId entityId)
        {
            if (_entitiesById.ContainsKey(entityId))
            {
                return _entitiesById[entityId].LocalStep >= 0;
            }
            return false;
        }

        public TEntity GetExistingEntity(EntityId entityId)
        {
            if (_entitiesById.TryGetValue(entityId, out var entity) && entity.LocalStep >= 0)
            {
                return entity;
            }

            throw new Exception("Entity don't not exist.");
        }

        public void StepForward()
        {
            // Using for loop to be able to register new entities during simulation
            for (int i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[i];
                entity.StepForward();
            }
        }

        public void SaveStep()
        {
            foreach (var entity in _entities)
            {
                entity.SaveStep();
            }

            CurrentStep += 1;
        }

        public void Rollback(int steps)
        {
            foreach (var entity in _entities)
            {
                entity.Rollback(steps);
            }

            CurrentStep -= steps;

            LoseTrackOfMissingEntities();
        }

        private void LoseTrackOfMissingEntities()
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                var entity = _entities[i];
                if (entity.LocalStep <= 0)
                {
                    _entities.RemoveAt(i);
                    _entitiesById.Remove(_idsByEntity[entity]);
                    _idsByEntity.Remove(entity);
                }
            }
        }
    }
}
