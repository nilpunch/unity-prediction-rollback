using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Character : UnityEntity,
        ICommandTarget<CharacterMoveCommand>,
        ICommandTarget<CharacterShootCommand>
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private CharacterMovement _unityCharacterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

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

        private void LateUpdate()
        {
            _renderer.enabled = UnitySimulation.CharacterWorld.IsAlive(Id);
        }

        public void ExecuteCommand(in CharacterShootCommand command)
        {
            _characterShooting.Shoot(command.Direction);
        }
    }
}
