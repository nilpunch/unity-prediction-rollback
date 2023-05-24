using System;
using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity, IReusableEntity
    {
        private readonly ChangeHistory<EntityStatus> _activityHistory = new ChangeHistory<EntityStatus>(EntityStatus.Active);
        private int _birthStep;

        public EntityStatus Status
        {
            get
            {
                return _activityHistory.LastSavedValue;
            }
        }

        public int LocalStep { get; set; }
        public int GlobalStep => _birthStep + LocalStep;

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void ResetLife(int birthStep)
        {
            _birthStep = birthStep;
            LocalStep = 0;
            LocalRollbacks.Rollback(LocalReversibleHistories.StepsSaved);
            LocalReversibleHistories.Rollback(LocalReversibleHistories.StepsSaved);
            _activityHistory.Rollback(LocalReversibleHistories.StepsSaved);
            OnBeginExist();
        }

        public void Sleep()
        {
            if (Status != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake to fall asleep!");
            }

            _activityHistory.Value = EntityStatus.Inactive;
            OnDeactivate();
        }

        public void StepForward()
        {
            if (Status != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake!");
            }

            LocalSimulations.StepForward();
        }

        public void SubmitStep()
        {
            if (Status != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake!");
            }

            LocalReversibleHistories.SubmitStep();
            _activityHistory.SubmitStep();

            LocalStep += 1;
        }

        public void Rollback(int steps)
        {
            int stepsToRollback = Math.Min(LocalReversibleHistories.StepsSaved, steps);
            LocalRollbacks.Rollback(stepsToRollback);
            LocalReversibleHistories.Rollback(stepsToRollback);
            _activityHistory.Rollback(stepsToRollback);

            LocalStep -= steps;

            if (LocalReversibleHistories.StepsSaved == 0)
            {
                OnBeginExist();
            }
            else if (Status == EntityStatus.Inactive)
            {
                OnDeactivate();
            }
            else
            {
                OnActivated();
            }
        }

        protected virtual void OnActivated() { }
        protected virtual void OnDeactivate() { }
        protected virtual void OnBeginExist() { }
    }
}
