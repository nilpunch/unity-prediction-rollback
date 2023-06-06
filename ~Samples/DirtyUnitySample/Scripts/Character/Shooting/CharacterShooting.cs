using UnityEngine;

namespace UPR.Samples
{
    public class CharacterShooting : MonoBehaviour, ISimulation, IHistory, IRollback, IRebase
    {
        [SerializeField] private float _delaySeconds = 0.2f;

        private readonly Timer _timer = new Timer();

        private Vector3 _shootingDirection;
        private bool _isShooting;

        public void SetShootingDirection(Vector3 direction)
        {
            _shootingDirection = direction;
        }

        public void EnableShooting()
        {
            _isShooting = true;
        }

        public void DisableShooting()
        {
            _isShooting = false;
        }

        public void StepForward()
        {
            _timer.StepForward();

            float currentTime = _timer.CurrentTick * UnitySimulation.SimulationSpeed.SecondsPerTick;

            if (!_isShooting || currentTime < _delaySeconds)
            {
                return;
            }

            _timer.Reset();

            var bullet = UnitySimulation.BulletsFactory.Create();
            bullet.Launch(transform.position, _shootingDirection);
        }

        public void SaveStep()
        {
            _timer.SaveStep();
        }

        public void Rollback(int steps)
        {
            _timer.Rollback(steps);
        }

        public void ForgetFromBeginning(int steps)
        {
            _timer.ForgetFromBeginning(steps);
        }
    }
}
