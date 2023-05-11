using System;
using System.Collections.Generic;

namespace UPR
{
    public class EntityWorld : IEntityWorld, IHistory, ISimulation, IRollback
    {
        private readonly Dictionary<EntityId, EntityLifetime> _entities = new Dictionary<EntityId, EntityLifetime>();

        public int CurrentStep { get; private set; }

        public void RegisterEntity(IEntity entity)
        {
            RegisterEntityAtStep(CurrentStep, entity);
        }

        public void RegisterEntityAtStep(int step, IEntity entity)
        {
            _entities.Add(entity.Id, new EntityLifetime(entity, step));
        }

        public void KillEntity(EntityId entityId)
        {
            if (_entities[entityId].BirthStep == CurrentStep)
            {
                _entities.Remove(entityId);
            }
            else
            {
                _entities[entityId].KillAtStep(CurrentStep);
            }
        }

        public IEntity FindAliveEntity(EntityId entityId)
        {
            if (_entities.TryGetValue(entityId, out var entityLifetime) && entityLifetime.IsAliveAtStep(CurrentStep))
            {
                return entityLifetime.Entity;
            }

            return null;
        }

        public void StepForward()
        {
            foreach (var lifetime in _entities.Values)
            {
                if (lifetime.IsAliveAtStep(CurrentStep))
                {
                    lifetime.Entity.StepForward();
                }
            }
        }

        public void SaveStep()
        {
            foreach (var lifetime in _entities.Values)
            {
                if (lifetime.IsAliveAtStep(CurrentStep))
                {
                    lifetime.Entity.SaveStep();
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

            // Lose track of entities that just born or not even born at target tick
            foreach (var lifetime in _entities.Values)
            {
                if (lifetime.BirthStep >= targetTick)
                {
                    s_entitiesToRemove.Add(lifetime.Entity.Id);
                }
            }
            foreach (var entityId in s_entitiesToRemove)
            {
                _entities.Remove(entityId);
            }
            s_entitiesToRemove.Clear();

            foreach (var lifetime in _entities.Values)
            {
                if (lifetime.IsAliveAtStep(targetTick) && lifetime.IsDeadAtStep(CurrentStep)) // Currently dead, but was alive
                {
                    int entityAgeAtTargetTick = targetTick - lifetime.BirthStep;
                    int entityAgeAtDeath = lifetime.DeathStep - lifetime.BirthStep;
                    lifetime.Resurrect();
                    lifetime.Entity.Rollback(entityAgeAtDeath - entityAgeAtTargetTick);
                }
                else if (lifetime.IsAliveAtStep(targetTick)) // Was alive and currently alive
                {
                    lifetime.Entity.Rollback(steps);
                }
            }

            CurrentStep -= steps;
        }
    }
}
