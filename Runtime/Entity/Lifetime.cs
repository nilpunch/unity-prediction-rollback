using System;
using UnityEngine;

namespace UPR
{
    public class Lifetime
    {
        private int _deathStep = int.MaxValue;
        private int _currentStep;
        private int _stepsAlive;

        public bool IsAlive => _currentStep >= 0 && _currentStep < _deathStep;

        public bool IsVolatile => _currentStep <= 0;

        public void Reset()
        {
            _currentStep = 0;
            _stepsAlive = 0;
            _deathStep = int.MaxValue;
        }

        public void Kill()
        {
            if (!IsAlive)
                throw new Exception("What's dead can't be killed.");

            _deathStep = _currentStep;
        }

        public void SaveStep()
        {
            if (IsAlive)
            {
                _stepsAlive += 1;
            }

            _currentStep += 1;
        }

        public int AliveStepsToRollback(int steps)
        {
            if (!IsAlive)
            {
                int howLongWeAreDead = _currentStep - _deathStep;
                int needToRollback = steps - howLongWeAreDead;
                int canRollbackSteps = Mathf.Max(0, Mathf.Min(needToRollback, _stepsAlive));
                return canRollbackSteps;
            }
            else
            {
                int canRollbackSteps = Mathf.Min(steps, _stepsAlive);
                return canRollbackSteps;
            }
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steps));
            }

            int aliveStepsToRollback = AliveStepsToRollback(steps);
            _stepsAlive -= aliveStepsToRollback;
            _currentStep -= steps;

            if (_currentStep <= _deathStep)
            {
                _deathStep = int.MaxValue;
            }
        }
    }
}
