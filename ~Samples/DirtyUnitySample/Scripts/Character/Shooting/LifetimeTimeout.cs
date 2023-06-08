using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class LifetimeTimeout : MonoBehaviour, ISimulation, IHistory, IRollback
    {
        [SerializeField] private float _lifetimeLengthSeconds = 2f;
        [SerializeField] private Lifetime _lifetime;

        private readonly Timer _timer = new Timer();

        public void ResetTimer()
        {
            _timer.Reset();
        }

        public void StepForward()
        {
            _timer.StepForward();

            float elapsedTime = _timer.CurrentTick * UnitySimulation.SimulationSpeed.SecondsPerTick;

            if (elapsedTime >= _lifetimeLengthSeconds)
            {
                _lifetime.IsAlive = false;
            }
        }

        public void SaveStep()
        {
            _timer.SaveStep();
        }

        public void Rollback(int steps)
        {
            _timer.Rollback(steps);
        }
    }
}
