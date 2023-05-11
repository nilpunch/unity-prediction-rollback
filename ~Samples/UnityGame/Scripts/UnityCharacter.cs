using UnityEngine;

namespace UPR.Samples
{
    public class UnityCharacter : UnityEntity,
        ICommandTarget<CharacterMoveCommand>
    {
        [SerializeField] private UnityCharacterMovement _unityCharacterMovement;

        private void Start()
        {
            var characterInventoryReversibleHistory = new ReversibleMemoryHistory<CharacterMovementMemory>(_unityCharacterMovement);

            LocalSimulations.AddSimulation(_unityCharacterMovement);

            LocalReversibleHistories.AddHistory(characterInventoryReversibleHistory);
        }

        public void ExecuteCommand(in CharacterMoveCommand command)
        {
            _unityCharacterMovement.SetMovement(command.MoveDirection);
        }
    }
}
