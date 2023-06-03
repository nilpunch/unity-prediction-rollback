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

        public void Launch(Vector3 position, Vector3 direction)
        {
            _entityTransform.Position = position;
            _characterMovement.SetMoveDirection(direction);
            _lifetime.IsAlive = true;
            _lifetimeTimeout.ResetTimer();
        }
    }
}
