using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity
    {
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private CharacterMovement _characterMovement;

        public EntityTransform EntityTransform => _entityTransform;

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

        public void Launch(Vector3 position, Vector3 direction)
        {
            _entityTransform.Position = position;
            _characterMovement.SetMoveDirection(direction);
            _lifetime.IsAlive = true;
        }
    }
}
