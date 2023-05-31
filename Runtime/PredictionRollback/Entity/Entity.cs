﻿using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        public int LocalStep { get; set; }

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected Histories LocalHistories { get; } = new Histories();

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
            int stepsToRollback = Math.Max(Math.Min(LocalStep, steps), 0);
            LocalRollbacks.Rollback(stepsToRollback);

            LocalStep -= steps;
        }
    }
}
