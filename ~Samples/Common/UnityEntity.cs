using System;
using System.Linq;
using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    [DisallowMultipleComponent]
    public class UnityEntity : MonoBehaviour, IReusableEntity, ISimulation, IHistory, IRollback, IRebase, ITickCounter
    {
        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected Histories LocalHistories { get; } = new Histories();

        protected Rebases LocalRebases { get; } = new Rebases();

        public int CurrentTick { get; private set; }

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
            int stepsToRollback = Math.Max(CurrentTick, 0);
            LocalRollbacks.Rollback(stepsToRollback);
            CurrentTick = 0;
        }

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
