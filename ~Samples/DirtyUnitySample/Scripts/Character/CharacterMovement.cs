using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : MonoBehaviour, ISimulation
    {
        [SerializeField] private float _speed = 5f;

        private Vector3 _moveDirection = Vector3.zero;

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _moveDirection = moveDirection;
        }

        public void StepForward()
        {
            transform.position += _moveDirection * (UnitySimulation.SimulationSpeed.SecondsPerTick * _speed);
        }
    }
}
