using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        private readonly ChangeHistory<EntityStatus> _activityHistory = new ChangeHistory<EntityStatus>(EntityStatus.Active);
        private readonly int _birthStep;

        protected Entity(int birthStep)
        {
            _birthStep = birthStep;
        }

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

        public void Sleep()
        {
            if (Status != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake to fall asleep!");
            }

            _activityHistory.Value = EntityStatus.Inactive;
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
        }
    }
}
