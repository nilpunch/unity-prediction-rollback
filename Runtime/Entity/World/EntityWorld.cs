using System;
using System.Collections.Generic;
using UnityEngine;

namespace UPR
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, IHistory, ISimulation, IRollback where TEntity : IEntity
    {
        private readonly Dictionary<EntityId, TEntity> _entities = new Dictionary<EntityId, TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesToAddLater = new Dictionary<EntityId, TEntity>();

        private bool _isIteratingOverEntities;

        public int StepsSaved { get; private set; }

        public void RegisterEntity(TEntity entity)
        {
            if (_isIteratingOverEntities)
            {
                _entitiesToAddLater.Add(entity.Id, entity);
            }
            else
            {
                _entities.Add(entity.Id, entity);
            }
        }

        public bool IsAlive(EntityId entityId)
        {
            if (_entities.ContainsKey(entityId))
            {
                return _entities[entityId].IsAlive;
            }

            if (_entitiesToAddLater.ContainsKey(entityId))
            {
                return _entitiesToAddLater[entityId].IsAlive;
            }

            return false;
        }

        public TEntity FindAliveEntity(EntityId entityId)
        {
            if (_entities.TryGetValue(entityId, out var entity) && entity.IsAlive)
            {
                return entity;
            }

            if (_entitiesToAddLater.TryGetValue(entityId, out entity) && entity.IsAlive)
            {
                return entity;
            }

            return default;
        }


        public void StepForward()
        {
            _isIteratingOverEntities = true;
            foreach (var entity in _entities.Values)
            {
                entity.StepForward();
            }
            _isIteratingOverEntities = false;

            foreach (var entity in _entitiesToAddLater)
            {
                if (entity.Value.IsAlive)
                {
                    _entities.Add(entity.Key, entity.Value);
                }
            }
            _entitiesToAddLater.Clear();
        }

        public void SaveStep()
        {
            foreach (var entity in _entities.Values)
            {
                entity.SaveStep();
            }

            StepsSaved += 1;
        }

        public void Rollback(int steps)
        {
            if (steps > StepsSaved)
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");

            foreach (var entity in _entities.Values)
            {
                entity.Rollback(steps);
            }

            LoseTrackOfVolatileEntities();

            StepsSaved -= steps;
        }

        private readonly List<EntityId> _bufferEntitiesToRemove = new List<EntityId>();

        private void LoseTrackOfVolatileEntities()
        {
            foreach (var entity in _entities)
            {
                if (entity.Value.IsVolatile)
                {
                    _bufferEntitiesToRemove.Add(entity.Key);
                }
            }

            foreach (var entityId in _bufferEntitiesToRemove)
            {
                _entities.Remove(entityId);
            }

            _bufferEntitiesToRemove.Clear();
        }
    }
}
