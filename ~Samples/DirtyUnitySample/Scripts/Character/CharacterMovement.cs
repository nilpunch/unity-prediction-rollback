using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : FrequentlyChangedComponent<CharacterMovement.Memory>, ISimulation
    {
        public struct Memory
        {
            public Vector3 MoveDirection { get; set; }
        }

        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private float _speed = 5f;

        public void SetMoveDirection(Vector3 moveDirection)
        {
            Data.MoveDirection = moveDirection;
        }

        public void StepForward()
        {
            _entityTransform.Position += Data.MoveDirection * (UnitySimulation.SimulationSpeed.SecondsPerTick * _speed);
        }
    }
}
