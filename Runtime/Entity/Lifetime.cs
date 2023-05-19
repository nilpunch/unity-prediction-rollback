using System;

namespace UPR
{
    public class Lifetime : IRollback
    {
        private int _deathStep = int.MaxValue;
        private int _stepsAlive;

        public int TotalSteps { get; private set; }

        public bool IsAlive => TotalSteps >= 0 && TotalSteps < _deathStep;

        public void Reset()
        {
            TotalSteps = 0;
            _stepsAlive = 0;
            _deathStep = int.MaxValue;
        }

        public void Kill()
        {
            if (!IsAlive)
                throw new Exception("What's dead can't be killed.");

            _deathStep = TotalSteps;
        }

        public void NextStep()
        {
            if (IsAlive)
            {
                _stepsAlive += 1;
            }

            TotalSteps += 1;
        }

        public int AliveStepsToRollback(int steps)
        {
            if (!IsAlive)
            {
                int howLongWeAreDead = TotalSteps - _deathStep;
                int needToRollback = steps - howLongWeAreDead;
                int canRollbackSteps = Math.Max(0, Math.Min(needToRollback, _stepsAlive));
                return canRollbackSteps;
            }
            else
            {
                int canRollbackSteps = Math.Min(steps, _stepsAlive);
                return canRollbackSteps;
            }
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            int aliveStepsToRollback = AliveStepsToRollback(steps);
            _stepsAlive -= aliveStepsToRollback;
            TotalSteps -= steps;

            if (TotalSteps <= _deathStep)
            {
                _deathStep = int.MaxValue;
            }
        }
    }
}
