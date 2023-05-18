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

            var transformReversibleHistory = new ReversibleMemoryHistory<EntityTransform.Memory>(_entityTransform);
            LocalReversibleHistories.AddHistory(transformReversibleHistory);

            var movementReversibleHistory = new ReversibleMemoryHistory<CharacterMovement.Memory>(_characterMovement);
            LocalReversibleHistories.AddHistory(movementReversibleHistory);

            LocalSimulations.AddSimulation(_characterMovement);
        }

        public void ExecuteCommand(in CharacterMoveCommand command)
        {
            _characterMovement.SetMoveDirection(command.MoveDirection);
        }

        private void LateUpdate()
        {
            _renderer.enabled = IsAlive;
        }

        public void ExecuteCommand(in CharacterShootCommand command)
        {
            _characterShooting.Shoot(command.Direction);
        }
    }
}
