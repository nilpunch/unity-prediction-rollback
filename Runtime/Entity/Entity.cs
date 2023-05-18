using System;

namespace UPR
{
    public abstract class Entity : IEntity
    {
        private int _deathStep;
        private int _currentStep;

        protected Entity(EntityId id)
        {
            Id = id;
            _deathStep = int.MaxValue;
        }


        public EntityId Id { get; }

        public bool IsAlive => _currentStep >= 0 && _currentStep < _deathStep;

        public bool IsVolatile => _currentStep <= 0;

        public int StepsSaved => LocalReversibleHistories.StepsSaved;

        protected Simulations LocalSimulations { get; } = new Simulations();

        protected Rollbacks LocalRollbacks { get; } = new Rollbacks();

        protected ReversibleHistories LocalReversibleHistories { get; } = new ReversibleHistories();

        public void Kill()
        {
            if (!IsAlive)
                throw new Exception("What's dead can't be killed.");

            _deathStep = StepsSaved;
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

            _currentStep += 1;
        }

        public void Rollback(int steps)
        {
            if (!IsAlive)
            {
                int howLongWeAreDead = _currentStep - _deathStep;
                int needToRollback = steps - howLongWeAreDead;
                int canRollbackSteps = Math.Min(needToRollback, LocalReversibleHistories.StepsSaved);
                LocalReversibleHistories.Rollback(canRollbackSteps);
            }
            else
            {
                int canRollbackSteps = Math.Min(steps, LocalReversibleHistories.StepsSaved);
                LocalReversibleHistories.Rollback(canRollbackSteps);
            }

            _currentStep -= steps;

            if (_currentStep <= _deathStep)
            {
                _deathStep = int.MaxValue;
            }
        }
    }
}
