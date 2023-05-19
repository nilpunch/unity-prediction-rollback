using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        private readonly Lifetime _lifetime = new Lifetime();

        public int Step => _lifetime.TotalSteps;

        public bool IsAlive => _lifetime.IsAlive;

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void Kill()
        {
            _lifetime.Kill();
        }

        public void StepForward()
        {
            if (IsAlive)
            {
                LocalSimulations.StepForward();
            }
        }

        public void SaveStep()
        {
            if (IsAlive)
            {
                LocalReversibleHistories.SaveStep();
            }

            _lifetime.NextStep();
        }

        public void Rollback(int steps)
        {
            int aliveStepsToRollback = _lifetime.AliveStepsToRollback(steps);
            LocalRollbacks.Rollback(aliveStepsToRollback);
            LocalReversibleHistories.Rollback(aliveStepsToRollback);
            _lifetime.Rollback(steps);
        }
    }
}
