using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UPR
{
    public class EntityWorld : IEntityWorld, IHistory, ISimulation, IRollback
    {
        private struct EntityRegistration
        {
            public EntityRegistration(IEntity entity, int birthStep)
            {
                Entity = entity;
                BirthStep = birthStep;
            }

            public IEntity Entity { get; }
            public int BirthStep { get; }
        }

        private readonly Dictionary<EntityId, IEntity> _entities = new Dictionary<EntityId, IEntity>();
        private readonly Dictionary<EntityId, int> _entityDeath = new Dictionary<EntityId, int>();
        private readonly Dictionary<EntityId, int> _entityBirth = new Dictionary<EntityId, int>();

        private readonly Dictionary<EntityId, EntityRegistration> _entitiesToAdd = new Dictionary<EntityId, EntityRegistration>();

        public int CurrentStep { get; private set; }

        public void RegisterEntity(IEntity entity)
        {
            RegisterEntityAtStep(CurrentStep, entity);
        }

        public void RegisterEntityAtStep(int step, IEntity entity)
        {
            _entitiesToAdd.Add(entity.Id, new EntityRegistration(entity, step));
        }

        public void KillEntity(EntityId entityId)
        {
            if (_entities.ContainsKey(entityId))
            {
                _entityDeath.Add(entityId, CurrentStep);
                return;
            }

            if (_entitiesToAdd.ContainsKey(entityId))
            {
                _entitiesToAdd.Remove(entityId);
                return;
            }

            throw new Exception("Trying to kill unknown entity. EntityID: " + entityId);
        }

        public void SubmitEntities()
        {
            foreach (var (entityId, registration) in _entitiesToAdd)
            {
                if (_entities.ContainsKey(entityId))
                    throw new Exception("Trying to register already registered entity. EntityID: " + entityId);

                _entities.Add(entityId, registration.Entity);
                _entityBirth.Add(entityId, registration.BirthStep);
            }
            _entitiesToAdd.Clear();
        }

        public bool IsAlive(EntityId entityId)
        {
            if (_entities.ContainsKey(entityId))
            {
                return IsAliveAtStep(entityId, CurrentStep);
            }

            if (_entitiesToAdd.ContainsKey(entityId))
            {
                return true;
            }

            return false;
        }

        public IEntity FindAliveEntity(EntityId entityId)
        {
            if (_entities.TryGetValue(entityId, out var entity) && IsAlive(entityId))
            {
                return entity;
            }

            if (_entitiesToAdd.TryGetValue(entityId, out var registration))
            {
                return registration.Entity;
            }

            return null;
        }

        public void StepForward()
        {
            foreach (var entity in _entities.Values)
            {
                if (IsAlive(entity.Id))
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
                if (IsAlive(entity.Id))
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

                if (IsAliveAtStep(entityId, targetTick) && !IsAliveAtStep(entityId, CurrentStep)) // Currently dead, but was alive
                {
                    int howLongEntityDead = CurrentStep - _entityDeath[entityId];
                    _entityDeath.Remove(entityId);
                    entity.Rollback(steps - howLongEntityDead);
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
    }
}
