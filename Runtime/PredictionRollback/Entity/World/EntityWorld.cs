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
            return _entitiesById.ContainsKey(entityId);
        }

        public TEntity GetExistingEntity(EntityId entityId)
        {
            return _entitiesById[entityId];
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

            CurrentStep += 1;
        }

        public void Rollback(int steps)
        {
            foreach (var entity in _entities)
            {
                entity.Rollback(steps);
            }

            CurrentStep -= steps;

            LoseTrackOfNotBornEntities();
        }

        private void LoseTrackOfNotBornEntities()
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
