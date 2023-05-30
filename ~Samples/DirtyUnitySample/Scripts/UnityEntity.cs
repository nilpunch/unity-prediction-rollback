using System;
using UnityEngine;

namespace UPR.Samples
{
    public abstract class UnityEntity : MonoBehaviour, IEntity, IReusableEntity
    {
        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public int LocalStep { get; set; }

        public virtual bool CanBeReused => false;

        public void ResetLife()
        {
            LocalStep = 0;
            LocalRollbacks.Rollback(LocalReversibleHistories.StepsSaved);
            LocalReversibleHistories.Rollback(LocalReversibleHistories.StepsSaved);
        }

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
                LocalReversibleHistories.SaveStep();
            }

            LocalStep += 1;
        }

        public void Rollback(int steps)
        {
            int stepsToRollback = Math.Max(Math.Min(LocalStep, steps), 0);
            LocalRollbacks.Rollback(stepsToRollback);
            LocalReversibleHistories.Rollback(stepsToRollback);

            LocalStep -= steps;
        }
    }
}
