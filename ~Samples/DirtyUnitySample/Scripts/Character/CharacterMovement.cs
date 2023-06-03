using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : MonoBehaviour, ISimulation, IHistory, IRollback
    {
        [SerializeField] private float _speed = 5f;

        private ReversibleValue<Vector3> _moveDirection = new ReversibleValue<Vector3>(default);

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _moveDirection.Value = moveDirection;
        }

        public void StepForward()
        {
            transform.position += _moveDirection.Value * (UnitySimulation.SimulationSpeed.SecondsPerTick * _speed);
        }

        public void SaveStep()
        {
            _moveDirection.SaveStep();
        }

        public void Rollback(int steps)
        {
            _moveDirection.Rollback(steps);
        }
    }
}
