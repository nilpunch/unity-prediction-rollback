using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity, IReusableEntity
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private EntityTransform _entityTransform;
        [SerializeField] private CharacterMovement _characterMovement;

        private void Awake()
        {
            var transformReversibleHistory = new ReversibleMemoryHistory<EntityTransform.Memory>(_entityTransform);
            LocalReversibleHistories.AddHistory(transformReversibleHistory);

            var movementReversibleHistory = new ReversibleMemoryHistory<CharacterMovement.Memory>(_characterMovement);
            LocalReversibleHistories.AddHistory(movementReversibleHistory);

            LocalSimulations.AddSimulation(_characterMovement);
        }

        public void LateUpdate()
        {
            _renderer.enabled = UnitySimulation.BulletsWorld.IsAlive(Id);
            // _collider.enabled = UnitySimulation.BulletsWorld.IsAlive(Id);
        }

        public void Launch(Vector3 position, Vector3 direction)
        {
            _entityTransform.Position = position;
            _characterMovement.SetMoveDirection(direction);
            // _collider.enabled = true;
        }

        public void ChangeId(EntityId entityId)
        {
            Id = entityId;
        }
    }
}
