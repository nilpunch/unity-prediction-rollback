using UnityEngine;

namespace UPR.Samples
{
    public class Character : UnityEntity,
        ICommandTarget<CharacterMoveCommand>,
        ICommandTarget<CharacterShootCommand>
    {
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

        public EntityTransform EntityTransform => _entityTransform;
        public Lifetime Lifetime => _lifetime;

        public void ExecuteCommand(in CharacterMoveCommand command)
        {
            _characterMovement.SetMoveDirection(command.MoveDirection);
        }

        public void ExecuteCommand(in CharacterShootCommand command)
        {
            _characterShooting.Shoot(command.Direction);
        }
    }
}
