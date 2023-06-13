using System;
using UPR.PredictionRollback;

namespace UPR.Utils
{
    /// <summary>
    /// Code reusal.
    /// </summary>
    public abstract class Entity : ISimulation, IHistory, IRollback, IRebase, ITickCounter
    {
        public int CurrentTick { get; private set; }

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected Histories LocalHistories { get; } = new Histories();

        protected Rebases LocalRebases { get; } = new Rebases();

        public void StepForward()
        {
            if (CurrentTick >= 0)
            {
                LocalSimulations.StepForward();
            }
        }

        public void SaveStep()
        {
            if (CurrentTick >= 0)
            {
                LocalHistories.SaveStep();
            }

            CurrentTick += 1;
        }

        public void Rollback(int steps)
        {
            int stepsToRollback = Math.Max(Math.Min(CurrentTick, steps), 0);
            LocalRollbacks.Rollback(stepsToRollback);
            CurrentTick -= steps;
        }

        public void ForgetFromBeginning(int steps)
        {
            LocalRebases.ForgetFromBeginning(steps);
        }
    }
}
