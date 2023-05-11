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
            _characterMovement = new CharacterMovement();

            var characterInventoryReversibleHistory = new ReversibleMemoryHistory<CharacterInventoryMemory>(_characterInventory);
            var characterMovementReversibleHistory = new ReversibleMemoryHistory<CharacterInventoryMemory>(_characterInventory);

            LocalSimulations.AddSimulation(_characterMovement);

            LocalHistories.AddHistory(characterInventoryReversibleHistory);
            LocalHistories.AddHistory(characterMovementReversibleHistory);

            LocalRollbacks.AddRollback(characterInventoryReversibleHistory);
            LocalRollbacks.AddRollback(characterMovementReversibleHistory);
        }

        public void ExecuteCommand(in CharacterMoveCommand command)
        {
            _characterMovement.Move(command.MoveDirection);
        }

        public void ExecuteCommand(in CharacterInventoryCommand command)
        {
            _characterInventory.ChangeActiveSlot(command.ActiveSlot);
        }
    }
}
