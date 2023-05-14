using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : MonoBehaviour, ISimulation, IMemory<CharacterMovement.Memory>
    {
        public struct Memory
        {
            public Vector3 MoveDirection { get; set; }
        }

        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private float _speed = 5f;

        private Memory _memory;

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _memory.MoveDirection = moveDirection;
        }

        public void StepForward()
        {
            _entityTransform.Position += _memory.MoveDirection * (UnitySimulation.SimulationSpeed.SecondsPerTick * _speed);
        }

        public Memory Save()
        {
            return _memory;
        }

        public void Load(Memory memory)
        {
            _memory = memory;
        }
    }
}
