using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity, IReusableEntity
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private CharacterMovement _characterMovement;

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

        public void LateUpdate()
        {
            _renderer.enabled = IsAlive && !IsVolatile;
        }

        public void Launch(Vector3 position, Vector3 direction)
        {
            _entityTransform.Position = position;
            _characterMovement.SetMoveDirection(direction);
            _collider.enabled = true;
        }

        protected override void OnKilled()
        {
            _collider.enabled = false;
        }

        protected override void OnResurrected()
        {
            _collider.enabled = true;
        }
    }
}
