using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        private readonly ChangeHistory<EntityStatus> _activityHistory = new ChangeHistory<EntityStatus>(EntityStatus.Active);

        public EntityStatus CurrentStatus => _activityHistory.Value;

        public EntityStatus LastSavedStatus => _activityHistory.LastSavedValue;

        public int LocalStep { get; set; }

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void Sleep()
        {
            if (CurrentStatus != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake to fall asleep!");
            }

            _activityHistory.Value = EntityStatus.Inactive;
        }

        public void Wake(int stepsAsleep)
        {
            if (CurrentStatus != EntityStatus.Inactive)
            {
                throw new InvalidOperationException("Entity must sleep before being waked!");
            }

            _activityHistory.Value = EntityStatus.Active;
            LocalStep += stepsAsleep;
        }

        public void StepForward()
        {
            if (CurrentStatus != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake during step!");
            }

            LocalSimulations.StepForward();
        }

        public void SubmitStep()
        {
            if (LastSavedStatus != EntityStatus.Active)
            {
                throw new InvalidOperationException("Entity must be wake at the beginning of the step!");
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
