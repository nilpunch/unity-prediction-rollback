using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : ISimulation, IMemory<CharacterMovementMemory>
    {
        private CharacterMovementMemory _characterMovementMemory;

        public void Move(Vector3 moveDirection)
        {
            _characterMovementMemory.MoveDirection = moveDirection;
        }

        public void StepForward(long currentTick)
        {
            _characterMovementMemory.Position += _characterMovementMemory.MoveDirection * 0.1f;
        }

        public CharacterMovementMemory Save()
        {
            return _characterMovementMemory;
        }

        public void Load(in CharacterMovementMemory snapshot)
        {
            _characterMovementMemory = snapshot;
        }
    }
}
