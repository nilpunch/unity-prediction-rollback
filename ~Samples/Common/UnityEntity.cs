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
            var components = GetComponentsInChildren<MonoBehaviour>()
                .Where(component => component != this)
                .ToArray();

            foreach (ISimulation simulation in components.OfType<ISimulation>())
            {
                LocalSimulations.Add(simulation);
            }

            foreach (IRollback rollback in components.OfType<IRollback>())
            {
                LocalRollbacks.Add(rollback);
            }

            foreach (IHistory history in components.OfType<IHistory>())
            {
                LocalHistories.Add(history);
            }

            foreach (IRebase rebase in  components.OfType<IRebase>())
            {
                LocalRebases.Add(rebase);
            }

            foreach (var initialize in components.OfType<IInitialize>())
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
