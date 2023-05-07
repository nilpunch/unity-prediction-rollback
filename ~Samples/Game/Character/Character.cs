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

            LocalSimulations.AddSimulation(_characterMovement);
            LocalStateHistories.AddHistory(new StateHistory<CharacterInventoryMemory>(_characterInventory));
            LocalStateHistories.AddHistory(new StateHistory<CharacterMovementMemory>(_characterMovement));
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
