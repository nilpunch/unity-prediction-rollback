using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        private readonly Lifetime _lifetime = new Lifetime();

        public int Age => _lifetime.Age;

        public bool IsAlive => _lifetime.IsAlive;

        public int StepsSaved => LocalReversibleHistories.StepsSaved;

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

            _lifetime.SaveStep();
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
