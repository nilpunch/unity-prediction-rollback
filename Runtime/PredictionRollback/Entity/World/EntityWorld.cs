using System;
using System.Collections.Generic;

namespace UPR
{
    public class EntityWorld<TEntity> : IEntityWorld<TEntity>, ISimulation, IHistory, IRollback, IRebase where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Dictionary<EntityId, TEntity> _entitiesById = new Dictionary<EntityId, TEntity>();
        private readonly Dictionary<TEntity, EntityId> _idsByEntity = new Dictionary<TEntity, EntityId>();

        public int StepsSaved => CurrentTick - HistoryBeginningTick;

        private int HistoryBeginningTick { get; set; }
        private int CurrentTick { get; set; }

        public EntityWorld(int worldTick = 0)
        {
            HistoryBeginningTick = worldTick;
            CurrentTick = worldTick;
        }

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

            CurrentTick += 1;
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (steps > StepsSaved)
                throw new Exception($"Can't rollback that far. {nameof(StepsSaved)}: {StepsSaved}, Rollbacking: {steps}.");

            foreach (var entity in _entities)
            {
                entity.Rollback(steps);
            }

            CurrentTick -= steps;

            LoseTrackOfNotBornEntities();
        }

        public void ForgetFromBeginning(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (steps > StepsSaved)
                throw new Exception($"Can't forget that far. {nameof(StepsSaved)}: {StepsSaved}, Forgetting: {steps}.");

            HistoryBeginningTick += steps;

            foreach (var entity in _entities)
            {
                int entityHistoryBegin = CurrentTick - entity.LocalStep;
                int canForgetSteps = Math.Max(HistoryBeginningTick - entityHistoryBegin, 0);
                entity.ForgetFromBeginning(Math.Min(canForgetSteps, steps));
            }
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
