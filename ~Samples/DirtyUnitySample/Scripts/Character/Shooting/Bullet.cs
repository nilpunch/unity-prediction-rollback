using System;
using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private CharacterMovement _characterMovement;

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

        public void Launch(Vector3 position, Vector3 direction)
        {
            _entityTransform.Position = position;
            _characterMovement.SetMoveDirection(direction);
            _collider.enabled = true;
            _renderer.enabled = true;
        }

        protected override void OnDeactivate()
        {
            _collider.enabled = false;
            _renderer.enabled = false;
        }

        protected override void OnBeginExist()
        {
            _collider.enabled = false;
            _renderer.enabled = false;
        }

        protected override void OnActivated()
        {
            _collider.enabled = true;
            _renderer.enabled = true;
        }
    }
}
