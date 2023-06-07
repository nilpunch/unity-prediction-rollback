using UnityEngine;

namespace UPR.Samples
{
    public class CircleMovement : MonoBehaviour, IInitialize, ISimulation, IRollback
    {
        [SerializeField] private float _speedX = 5f;
        [SerializeField] private float _speedY = 5f;
        [SerializeField] private float _radiusX = 5f;
        [SerializeField] private float _radiusY = 5f;
        [SerializeField] private float _phaseX = 0f;
        [SerializeField] private float _phaseY = 0f;

        private int _tick;
        private Vector3 _initialPosition;

        public void Initialize()
        {
            _initialPosition = transform.position;
        }

        public void StepForward()
        {
            _tick += 1;
            float time = _tick * UnitySimulation.SimulationSpeed.SecondsPerTick;
            transform.position = _initialPosition + CalculateOffset(time);
        }

        public void Rollback(int steps)
        {
            _tick -= steps;

            float time = _tick * UnitySimulation.SimulationSpeed.SecondsPerTick;
            transform.position = _initialPosition + CalculateOffset(time);
        }

        private Vector3 CalculateOffset(float time)
        {
            return new Vector3(Mathf.Sin((time + _phaseX * Mathf.Deg2Rad) * _speedX) * _radiusX * 2f, Mathf.Sin((time + _phaseY * Mathf.Deg2Rad) * _speedY) * _radiusY, 0);
        }
    }
}
