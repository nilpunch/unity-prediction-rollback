using System;
using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity, IReusableEntity
    {
        private readonly Lifetime _lifetime = new Lifetime();

        public EntityId Id { get; private set; }

        public bool IsAlive => _lifetime.IsAlive;

        public bool IsVolatile => _lifetime.IsVolatile;

        public int StepsSaved => LocalReversibleHistories.StepsSaved;

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void ResetLife(EntityId newId)
        {
            Id = newId;
            LocalReversibleHistories.Rollback(LocalReversibleHistories.StepsSaved);

            bool wasDead = !_lifetime.IsAlive;
            _lifetime.Reset();
            if (wasDead)
            {
                OnResurrected();
            }
        }

        public void Kill()
        {
            _lifetime.Kill();
            OnKilled();
        }

        protected virtual void OnKilled() { }

        public void StepForward()
        {
            if (IsAlive)
            {
                LocalSimulations.StepForward();
            }
        }

        public void SaveStep()
        {
            if (IsAlive)
            {
                LocalReversibleHistories.SaveStep();
            }

            _lifetime.SaveStep();
        }

        public void Rollback(int steps)
        {
            int aliveStepsToRollback = _lifetime.AliveStepsToRollback(steps);
            LocalRollbacks.Rollback(aliveStepsToRollback);
            LocalReversibleHistories.Rollback(aliveStepsToRollback);

            bool wasDead = !_lifetime.IsAlive;
            _lifetime.Rollback(steps);
            if (wasDead && _lifetime.IsAlive)
            {
                OnResurrected();
            }
        }

        protected virtual void OnResurrected() { }
    }
}
