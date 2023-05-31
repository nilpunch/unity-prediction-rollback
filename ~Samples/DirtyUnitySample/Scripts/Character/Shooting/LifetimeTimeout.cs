using UnityEngine;

namespace UPR.Samples
{
    public class LifetimeTimeout : MonoBehaviour, ISimulation, IRollback, IHistory
    {
        [SerializeField] private float _lifetimeLengthSeconds = 2f;
        [SerializeField] private Lifetime _lifetime;

        private readonly ChangeHistory<int> _resetTick = new ChangeHistory<int>(0);

        private int _elapsedTicks;

        public void ResetTimer()
        {
            _resetTick.Value = _elapsedTicks;
        }

        public void StepForward()
        {
            _elapsedTicks += 1;

            int currentTick = _elapsedTicks - _resetTick.Value;

            float elapsedTime = currentTick * UnitySimulation.SimulationSpeed.SecondsPerTick;

            if (elapsedTime >= _lifetimeLengthSeconds)
            {
                _lifetime.IsAlive = false;
            }
        }

        public void Rollback(int steps)
        {
            _resetTick.Rollback(steps);
            _elapsedTicks -= steps;
        }

        public void SaveStep()
        {
            _resetTick.SaveStep();
        }
    }
}
