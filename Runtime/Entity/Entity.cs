﻿namespace UPR
{
    public abstract class Entity : IEntity
    {
        protected readonly Simulations LocalSimulations = new Simulations();
        protected readonly ReversibleHistories LocalReversibleHistories = new ReversibleHistories();
        protected readonly Rollbacks LocalRollbacks = new Rollbacks();

        protected Entity(EntityId id)
        {
            Id = id;
        }

        public EntityId Id { get; }

        public int CurrentStep => LocalReversibleHistories.CurrentStep;

        public void StepForward()
        {
            LocalSimulations.StepForward();
        }

        public void SaveStep()
        {
            LocalReversibleHistories.SaveStep();
        }

        public void Rollback(int steps)
        {
            LocalRollbacks.Rollback(steps);
        }
    }
}
