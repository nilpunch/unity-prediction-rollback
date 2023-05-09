using System;
using System.Collections.Generic;

namespace UPR
{
    public class EntitiesTimeline : ISimulation, IStateHistory
    {
        private Dictionary<EntityId, EntityLifetime> _entities;

        private int _currentTick;

        public void RegisterEntity(IEntity entity)
        {
            _entities.Add(entity.Id, new EntityLifetime(entity, _currentTick));
        }

        public void KillEntity(EntityId entityId)
        {
            _entities[entityId].KillAtTick(_currentTick);
        }

        public void StepForward(int currentTick)
        {
            _currentTick = currentTick;

            foreach (var lifetime in _entities.Values)
            {
                if (lifetime.IsAliveAtTick(_currentTick))
                {
                    lifetime.Entity.StepForward(_currentTick);
                }
            }
        }

        public int HistoryLength => _currentTick;

        public void SaveState()
        {
            foreach (var lifetime in _entities.Values)
            {
                if (lifetime.IsAliveAtTick(_currentTick))
                {
                    lifetime.Entity.SaveState();
                }
            }
        }

        private static List<EntityId> s_entitiesToRemove = new List<EntityId>();

        public void Rollback(int ticks)
        {
            if (ticks > _currentTick)
                throw new Exception($"Can't rollback that far. {nameof(HistoryLength)}: {HistoryLength}, Rollbacking: {ticks}.");

            if (ticks == 0)
            {
                foreach (var lifetime in _entities.Values)
                {
                    if (lifetime.IsAliveAtTick(_currentTick))
                    {
                        lifetime.Entity.Rollback(0);
                    }
                }

                return;
            }

            var targetTick = _currentTick - ticks;

            // Lose track of entities that not even born at target tick
            foreach (var lifetime in _entities.Values)
            {
                if (!lifetime.IsBornAtTick(targetTick))
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
                if (lifetime.IsDeadAtTick(_currentTick) && lifetime.IsAliveAtTick(targetTick)) // Currently dead, but was alive
                {
                    int entityAgeAtTargetTick = targetTick - lifetime.BirthTick;
                    int entityAgeAtDeath = lifetime.DeathTick - lifetime.BirthTick;
                    lifetime.Resurrect();
                    lifetime.Entity.Rollback(entityAgeAtDeath - entityAgeAtTargetTick);
                }
                else if (lifetime.IsAliveAtTick(_currentTick) && lifetime.IsAliveAtTick(targetTick)) // Was alive and currently alive
                {
                    lifetime.Entity.Rollback(ticks);
                }
            }
        }
    }
}
