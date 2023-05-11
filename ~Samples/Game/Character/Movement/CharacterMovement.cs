using UnityEngine;

namespace UPR.Samples
{
    public class CharacterMovement : ISimulation, IMemory<CharacterMovementMemory>
    {
        private CharacterMovementMemory _characterMovementMemory;

        private readonly SimulationSpeed _simulationSpeed;

        public CharacterMovement(SimulationSpeed simulationSpeed)
        {
            _simulationSpeed = simulationSpeed;
        }

        public void SetMovement(Vector3 moveDirection)
        {
            _characterMovementMemory.MoveDirection = moveDirection;
        }

        public void StepForward()
        {
            _characterMovementMemory.Position += _characterMovementMemory.MoveDirection * _simulationSpeed.SecondsPerTick;
        }

        public CharacterMovementMemory Save()
        {
            return _characterMovementMemory;
        }

        public void Load(in CharacterMovementMemory memory)
        {
            _characterMovementMemory = memory;
        }
    }
}
