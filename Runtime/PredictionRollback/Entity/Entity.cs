using System;

namespace UPR.PredictionRollback
{
    public abstract class Entity : IEntity, ISimulation, IHistory, IRollback, IRebase
    {
        public int SavedSteps { get; set; }

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected Histories LocalHistories { get; } = new Histories();

        protected Rebases LocalRebases { get; } = new Rebases();

        public void StepForward()
        {
            if (SavedSteps >= 0)
            {
                LocalSimulations.StepForward();
            }
        }

        public void SaveStep()
        {
            if (SavedSteps >= 0)
            {
                LocalHistories.SaveStep();
            }

            SavedSteps += 1;
        }

        public void Rollback(int steps)
        {
            int stepsToRollback = Math.Max(Math.Min(SavedSteps, steps), 0);
            LocalRollbacks.Rollback(stepsToRollback);
            SavedSteps -= steps;
        }

        public void ForgetFromBeginning(int steps)
        {
            LocalRebases.ForgetFromBeginning(steps);
        }
    }
}
