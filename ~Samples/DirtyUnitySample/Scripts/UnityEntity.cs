using System;
using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity
    {
        private int _deathStep = int.MaxValue;
        private int _currentStep;

        public EntityId Id { get; set; }

        public bool IsAlive => _currentStep >= 0 && _currentStep < _deathStep;
        public bool IsVolatile => _currentStep <= 0;

        public int StepsSaved => LocalReversibleHistories.StepsSaved;

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void ResetEntity()
        {
            _currentStep = 0;
            _deathStep = int.MaxValue;
            LocalReversibleHistories.Rollback(LocalReversibleHistories.StepsSaved);
        }

        public void Kill()
        {
            if (!IsAlive)
                throw new Exception("What's dead can't be killed.");

            _deathStep = StepsSaved;

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

            _currentStep += 1;
        }

        public void Rollback(int steps)
        {
            if (!IsAlive)
            {
                int howLongWeAreDead = _currentStep - _deathStep;
                int needToRollback = steps - howLongWeAreDead;
                int canRollbackSteps = Mathf.Max(0, Mathf.Min(needToRollback, LocalReversibleHistories.StepsSaved));
                LocalReversibleHistories.Rollback(canRollbackSteps);
            }
            else
            {
                int canRollbackSteps = Mathf.Min(steps, LocalReversibleHistories.StepsSaved);
                LocalReversibleHistories.Rollback(canRollbackSteps);
            }

            _currentStep -= steps;

            if (_deathStep != int.MaxValue && _currentStep <= _deathStep)
            {
                _deathStep = int.MaxValue;
                OnResurrected();
            }
        }

        protected virtual void OnResurrected() { }
    }
}
