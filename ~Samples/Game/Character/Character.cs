using System;

namespace UPR.Samples
{
    public class Character : Entity,
        ICommandTarget<CharacterMoveCommand>,
        ICommandTarget<CharacterInventoryCommand>
    {
        private readonly CharacterInventory _characterInventory;
        private readonly CharacterMovement _characterMovement;

        public Character(EntityId id) : base(id)
        {
            _characterInventory = new CharacterInventory();
            _characterMovement = new CharacterMovement(new SimulationSpeed(60));

            var characterInventoryReversibleHistory = new ReversibleMemoryHistory<CharacterInventoryMemory>(_characterInventory);
            var characterMovementReversibleHistory = new ReversibleMemoryHistory<CharacterInventoryMemory>(_characterInventory);

            LocalSimulations.AddSimulation(_characterMovement);

            LocalReversibleHistories.AddHistory(characterInventoryReversibleHistory);
            LocalReversibleHistories.AddHistory(characterMovementReversibleHistory);
        }

        public void ExecuteCommand(in CharacterMoveCommand command)
        {
            _characterMovement.SetMovement(command.MoveDirection);
        }

        public void ExecuteCommand(in CharacterInventoryCommand command)
        {
            _characterInventory.ChangeActiveSlot(command.ActiveSlot);
        }
    }
}
