using System;
using System.Linq;
using UnityEngine;

namespace UPR.Samples
{
    [DisallowMultipleComponent]
    public class UnityEntity : MonoBehaviour, IEntity, IReusableEntity, IRebase
    {
        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected Histories LocalHistories { get; } = new Histories();

        protected Rebases LocalRebases { get; } = new Rebases();

        public int LocalStep { get; private set; }

        public virtual bool CanBeReused => false;

        private void Awake()
        {
            var components = GetComponentsInChildren<MonoBehaviour>();

            var simulations = components.Where(component => component != this && component is ISimulation).Cast<ISimulation>();
            var rollbacks = components.Where(component => component != this && component is IRollback).Cast<IRollback>();
            var histories = components.Where(component => component != this && component is IHistory).Cast<IHistory>();
            var rebases = components.Where(component => component != this && component is IRebase).Cast<IRebase>();
            var initializes = components.Where(component => component is IInitialize).Cast<IInitialize>();

            foreach (ISimulation simulation in simulations)
            {
                LocalSimulations.Add(simulation);
            }

            foreach (IRollback rollback in rollbacks)
            {
                LocalRollbacks.Add(rollback);
            }

            foreach (IHistory history in histories)
            {
                LocalHistories.Add(history);
            }

            foreach (IRebase rebase in rebases)
            {
                LocalRebases.Add(rebase);
            }

            foreach (var initialize in initializes)
            {
                initialize.Initialize();
            }
        }

        public void FullyResetEntity()
        {
            int stepsToRollback = Math.Max(LocalStep, 0);
            LocalRollbacks.Rollback(stepsToRollback);
            LocalStep = 0;
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

        public void ForgetFromBeginning(int steps)
        {
            LocalRebases.ForgetFromBeginning(steps);
        }
    }
}
