using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        public int LocalStep { get; set; }

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected Histories LocalHistories { get; } = new Histories();

        protected Rebases LocalRebases { get; } = new Rebases();

        public void StepForward()
        {
            if (LocalStep >= 0)
            {
                LocalSimulations.StepForward();
            }
        }

        public void SaveStep()
        {
            if (LocalStep >= 0)
            {
                LocalHistories.SaveStep();
            }

            LocalStep += 1;
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steps));
            }

            int stepsToRollback = Math.Max(Math.Min(LocalStep, steps), 0);
            LocalRollbacks.Rollback(stepsToRollback);

            LocalStep -= steps;
        }

        public void ForgetFromBeginning(int steps)
        {
            LocalRebases.ForgetFromBeginning(steps);
        }
    }
}
