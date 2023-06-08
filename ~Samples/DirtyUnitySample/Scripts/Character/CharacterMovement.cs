using UnityEngine;
using UPR.PredictionRollback;
using UPR.Utils;

namespace UPR.Samples
{
    [RequireComponent(typeof(EntityTransform))] // Component does not save position by itself. So, we need EntityTransform
    public class CharacterMovement : MonoBehaviour, ISimulation, IHistory, IRollback, IRebase
    {
        [SerializeField] private float _speed = 5f;

        private readonly RarelyChangingValue<Vector3> _moveDirection = new RarelyChangingValue<Vector3>(Vector3.zero);

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

        public void ForgetFromBeginning(int steps)
        {
            _moveDirection.ForgetFromBeginning(steps);
        }
    }
}
