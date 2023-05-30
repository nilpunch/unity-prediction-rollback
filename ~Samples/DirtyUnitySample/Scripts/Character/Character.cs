﻿using UnityEngine;

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

        private void Awake()
        {
            _entityTransform.Init();
            _lifetime.Init();

            var transformReversibleHistory = new MemoryHistory<EntityTransform.Memory>(_entityTransform);
            LocalReversibleHistories.AddReversibleHistory(transformReversibleHistory);

            var movementReversibleHistory = new MemoryHistory<CharacterMovement.Memory>(_characterMovement);
            LocalReversibleHistories.AddReversibleHistory(movementReversibleHistory);
            LocalReversibleHistories.AddReversibleHistory(_lifetime);

            LocalSimulations.AddSimulation(_characterMovement);
        }

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
