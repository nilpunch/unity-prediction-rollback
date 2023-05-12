using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UPR
{
    public class EntityWorld : IEntityWorld, IHistory, ISimulation, IRollback
    {
        private readonly Dictionary<EntityId, IEntity> _entities = new Dictionary<EntityId, IEntity>();
        private readonly Dictionary<EntityId, int> _entityDeath = new Dictionary<EntityId, int>();
        private readonly Dictionary<EntityId, int> _entityBirth = new Dictionary<EntityId, int>();

        private readonly List<(IEntity entity, int birthStep)> _entitiesToAdd = new List<(IEntity entity, int birthStep)>();

        public int CurrentStep { get; private set; }

        public void RegisterEntity(IEntity entity)
        {
            RegisterEntityAtStep(CurrentStep, entity);
        }

        public void RegisterEntityAtStep(int step, IEntity entity)
        {
            _entitiesToAdd.Add((entity, step));
        }

        public void SubmitEntities()
        {
            foreach (var (entity, birthStep) in _entitiesToAdd)
            {
                if (entity.IsAlive)
                {
                    _entities.Add(entity.Id, entity);
                    _entityBirth.Add(entity.Id, birthStep);
                }
            }
            _entitiesToAdd.Clear();

            foreach (var entity in _entities)
            {
                if (!entity.Value.IsAlive && !_entityDeath.ContainsKey(entity.Key))
                {
                    _entityDeath.Add(entity.Key, CurrentStep);
                }
            }
        }

        public IEntity FindAliveEntity(EntityId entityId)
        {
            if (_entities.TryGetValue(entityId, out var entity) && entity.IsAlive)
            {
                return entity;
            }

            return null;
        }

        public bool IsExistsInHistory(EntityId entityId)
        {
            return _entities.ContainsKey(entityId);
        }

        public void StepForward()
        {
            foreach (var entity in _entities.Values)
            {
                if (entity.IsAlive)
                {
                    entity.StepForward();
                }
            }
        }

        public void SaveStep()
        {
            SubmitEntities();

            foreach (var entity in _entities.Values)
            {
                if (entity.IsAlive)
                {
                    entity.SaveStep();
                }
            }

            CurrentStep += 1;
        }

        private static List<EntityId> s_entitiesToRemove = new List<EntityId>();

        public void Rollback(int steps)
        {
            if (steps > CurrentStep)
                throw new Exception($"Can't rollback that far. {nameof(CurrentStep)}: {CurrentStep}, Rollbacking: {steps}.");

            var targetTick = CurrentStep - steps;

            LoseTrackOfEntitiesAfterTick(targetTick);

            foreach (var pair in _entities)
            {
                var entityId = pair.Key;
                var entity = pair.Value;

                if (IsAliveAtStep(entityId, targetTick) && IsDeadAtStep(entityId, CurrentStep)) // Currently dead, but was alive
                {
                    int howLongEntityDead = CurrentStep - _entityDeath[entityId];
                    _entityDeath.Remove(entityId);
                    entity.Rollback(steps - howLongEntityDead);
                    entity.Resurrect();
                }
                else if (IsAliveAtStep(entityId, targetTick)) // Was alive and currently alive
                {
                    entity.Rollback(steps);
                }
            }

            CurrentStep -= steps;
        }

        private void LoseTrackOfEntitiesAfterTick(int targetStep)
        {
            foreach (var entity in _entities)
            {
                int birthStep = _entityBirth[entity.Key];
                if (birthStep >= targetStep)
                {
                    s_entitiesToRemove.Add(entity.Key);
                }
            }

            foreach (var entityId in s_entitiesToRemove)
            {
                _entities.Remove(entityId);
                _entityBirth.Remove(entityId);
                _entityDeath.Remove(entityId);
            }

            s_entitiesToRemove.Clear();
        }

        private bool IsAliveAtStep(EntityId entityId, int step)
        {
            int birthStep = _entityBirth[entityId];
            if (_entityDeath.TryGetValue(entityId, out int deathStep))
            {
                return step >= birthStep && step < deathStep;
            }

            return step >= birthStep;
        }

        private bool IsDeadAtStep(EntityId entityId, int step)
        {
            if (_entityDeath.TryGetValue(entityId, out int deathStep))
            {
                return step >= deathStep;
            }

            return false;
        }
    }
}
