using System;
using System.Collections.Generic;
using UnityEngine;

namespace UPR
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, IHistory, ISimulation, IRollback where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesById = new Dictionary<EntityId, TEntity>();

        public int StepsSaved { get; private set; }

        public void RegisterEntity(TEntity entity)
        {
            _entities.Add(entity);
            _entitiesById.Add(entity.Id, entity);
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

            StepsSaved += 1;
        }

        public void Rollback(int steps)
        {
            if (steps > StepsSaved)
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");

            foreach (var entity in _entities)
            {
                entity.Rollback(steps);
            }

            StepsSaved -= steps;

            LoseTrackOfVolatileEntities();
        }

        private void LoseTrackOfVolatileEntities()
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                var entity = _entities[i];
                if (entity.IsVolatile)
                {
                    _entities.RemoveAt(i);
                    _entitiesById.Remove(entity.Id);
                }
            }
        }
    }
}
