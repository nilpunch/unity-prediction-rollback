using System;
using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity, IReusableEntity
    {
        private readonly Lifetime _lifetime = new Lifetime();

        public int Step => _lifetime.TotalSteps;

        public bool IsAlive => _lifetime.IsAlive;

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void ResetLife()
        {
            LocalReversibleHistories.Rollback(LocalReversibleHistories.StepsSaved);
            _lifetime.Reset();
            OnBeginExists();
        }

        public void Kill()
        {
            _lifetime.Kill();
            OnKilled();
        }

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

            _lifetime.NextStep();
        }

        public void Rollback(int steps)
        {
            int aliveStepsToRollback = _lifetime.AliveStepsToRollback(steps);
            LocalRollbacks.Rollback(aliveStepsToRollback);
            LocalReversibleHistories.Rollback(aliveStepsToRollback);
            _lifetime.Rollback(steps);

            if (Step == 0)
                OnBeginExists();
            else if (_lifetime.IsAlive)
                OnAlive();
            else
                OnKilled();
        }

        protected virtual void OnAlive() { }
        protected virtual void OnKilled() { }
        protected virtual void OnBeginExists() { }
    }
}
