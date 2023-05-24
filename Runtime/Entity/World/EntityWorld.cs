using System;
using System.Collections.Generic;

namespace UPR
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, IHistory, ISimulation, IRollback where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesById = new Dictionary<EntityId, TEntity>();
        private readonly Dictionary<TEntity, EntityId> _idsByEntity = new Dictionary<TEntity, EntityId>();

        private int _currentStep;

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

        public EntityStatus GetStatus(EntityId entityId)
        {
            if (_entitiesById.ContainsKey(entityId))
            {
                return _entitiesById[entityId].Status;
            }
            return EntityStatus.Inactive;
        }

        public TEntity FindWakeEntity(EntityId entityId)
        {
            if (_entitiesById.TryGetValue(entityId, out var entity) && entity.Status == EntityStatus.Active)
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
                var entity = _entities[i];
                if (entity.Status == EntityStatus.Active)
                {
                    entity.StepForward();
                }
            }
        }

        public void SubmitStep()
        {
            foreach (var entity in _entities)
            {
                if (entity.Status == EntityStatus.Active)
                {
                    entity.SubmitStep();
                }
            }

            _currentStep += 1;
        }

        public void Rollback(int steps)
        {
            foreach (var entity in _entities)
            {
                if (entity.GlobalStep < _currentStep)
                {
                    int howLongEntityInactive = _currentStep - entity.GlobalStep;
                    entity.Rollback(Math.Max(steps - howLongEntityInactive, 0));
                }
                else if (entity.GlobalStep >= _currentStep - steps)
                {
                    entity.Rollback(steps);
                }
            }

            _currentStep -= steps;

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
