using System;
using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity, IReusableEntity
    {
        private readonly ChangeHistory<EntityStatus> _activityHistory = new ChangeHistory<EntityStatus>(EntityStatus.Active);

        public EntityStatus CurrentStatus => _activityHistory.Value;

        public bool HasUnsavedChanges { get; private set; }

        public int LocalStep { get; set; }

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void ResetLife()
        {
            LocalStep = 0;
            LocalRollbacks.Rollback(LocalReversibleHistories.StepsSaved);
            LocalReversibleHistories.Rollback(LocalReversibleHistories.StepsSaved);
            _activityHistory.Rollback(LocalReversibleHistories.StepsSaved);
            OnBeginExist();
        }

        public void Sleep()
        {
            if (CurrentStatus != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake to fall asleep!");
            }

            _activityHistory.Value = EntityStatus.Inactive;
            HasUnsavedChanges = true;
            OnDeactivate();
        }

        public void Wake(int stepsAsleep)
        {
            if (CurrentStatus != EntityStatus.Inactive)
            {
                throw new InvalidOperationException("Entity must sleep before being waked!");
            }

            _activityHistory.Value = EntityStatus.Active;
            LocalStep += stepsAsleep;
            HasUnsavedChanges = true;
        }

        public void StepForward()
        {
            if (CurrentStatus != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake!");
            }

            LocalSimulations.StepForward();
            HasUnsavedChanges = true;
        }

        public void SaveStep()
        {
            LocalReversibleHistories.SaveStep();
            _activityHistory.SaveStep();

            LocalStep += 1;
            HasUnsavedChanges = false;
        }

        public void Rollback(int steps)
        {
            int stepsToRollback = Math.Min(LocalReversibleHistories.StepsSaved, steps);
            LocalRollbacks.Rollback(stepsToRollback);
            LocalReversibleHistories.Rollback(stepsToRollback);
            _activityHistory.Rollback(stepsToRollback);

            LocalStep -= steps;
            HasUnsavedChanges = false;

            if (LocalReversibleHistories.StepsSaved == 0)
            {
                OnBeginExist();
            }
            else if (CurrentStatus == EntityStatus.Inactive)
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
