using UnityEngine;

namespace UPR.Samples
{
    public class Character : UnityEntity,
        ICommandTarget<CharacterMoveCommand>,
        ICommandTarget<CharacterShootCommand>
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

        public EntityTransform EntityTransform => _entityTransform;

        private void Awake()
        {
            _entityTransform.Init();

            var transformReversibleHistory = new MemoryHistory<EntityTransform.Memory>(_entityTransform);
            LocalReversibleHistories.AddHistory(transformReversibleHistory);

            var movementReversibleHistory = new MemoryHistory<CharacterMovement.Memory>(_characterMovement);
            LocalReversibleHistories.AddHistory(movementReversibleHistory);

            LocalSimulations.AddSimulation(_characterMovement);
        }

        public void ExecuteCommand(in CharacterMoveCommand command)
        {
            _characterMovement.SetMoveDirection(command.MoveDirection);
        }

        private void LateUpdate()
        {
            _renderer.enabled = Status == EntityStatus.Active;
        }

        public void ExecuteCommand(in CharacterShootCommand command)
        {
            _characterShooting.Shoot(command.Direction);
        }
    }
}
