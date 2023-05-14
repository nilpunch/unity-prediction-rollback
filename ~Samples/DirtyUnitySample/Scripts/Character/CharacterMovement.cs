using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : MonoBehaviour, ISimulation, IMemory<CharacterMovementMemory>
    {
        [SerializeField] private float _speed = 5f;

        private CharacterMovementMemory _memory;

        public Vector3 Position => _memory.Position;

        private void Awake()
        {
            _memory.Position = transform.position;
        }

        private void LateUpdate()
        {
            SyncTransform();
        }

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _memory.MoveDirection = moveDirection;
        }

        public void SetPosition(Vector3 position)
        {
            _memory.Position = position;
            SyncTransform();
        }

        public void StepForward()
        {
            _memory.Position += _memory.MoveDirection * (UnitySimulation.SimulationSpeed.SecondsPerTick * _speed);
            SyncTransform();
        }

        public CharacterMovementMemory Save()
        {
            return _memory;
        }

        public void Load(in CharacterMovementMemory memory)
        {
            _memory = memory;
            SyncTransform();
        }

        private void SyncTransform()
        {
            transform.position = _memory.Position;
        }
    }
}
