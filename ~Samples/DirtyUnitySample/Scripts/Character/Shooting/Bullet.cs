using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity
    {
        [SerializeField] private LifetimeTimeout _lifetimeTimeout;
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private CharacterMovement _characterMovement;

        public override bool CanBeReused => !_lifetime.IsAlive;

        private void Awake()
        {
            _entityTransform.Init();

            _lifetime.Init();
            LocalHistories.Add(_lifetime);
            LocalRollbacks.Add(_lifetime);

            var transformReversibleHistory = new MemoryHistory<EntityTransform.Memory>(_entityTransform);
            LocalHistories.Add(transformReversibleHistory);
            LocalRollbacks.Add(transformReversibleHistory);

            var movementReversibleHistory = new MemoryHistory<CharacterMovement.Memory>(_characterMovement);
            LocalHistories.Add(movementReversibleHistory);
            LocalRollbacks.Add(movementReversibleHistory);

            LocalHistories.Add(_lifetimeTimeout);
            LocalRollbacks.Add(_lifetimeTimeout);
            LocalSimulations.Add(_lifetimeTimeout);

            LocalSimulations.Add(_characterMovement);
        }

        public void Launch(Vector3 position, Vector3 direction)
        {
            _entityTransform.Position = position;
            _characterMovement.SetMoveDirection(direction);
            _lifetime.IsAlive = true;
            _lifetimeTimeout.ResetTimer();
        }
    }
}
