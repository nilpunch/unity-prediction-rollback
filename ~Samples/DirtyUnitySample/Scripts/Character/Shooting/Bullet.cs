using Tools;
using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity, ICachedEntity
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private CharacterMovement _unityCharacterMovement;

        public CharacterMovement Movement => _unityCharacterMovement;

        private void Awake()
        {
            var characterInventoryReversibleHistory = new ReversibleMemoryHistory<CharacterMovementMemory>(_unityCharacterMovement);

            LocalSimulations.AddSimulation(_unityCharacterMovement);
            LocalReversibleHistories.AddHistory(characterInventoryReversibleHistory);
        }

        public void LateUpdate()
        {
            Debug.Log(LocalReversibleHistories.CurrentStep);
            _renderer.enabled = UnitySimulation.BulletsWorld.IsAlive(Id);
        }

        public void Launch(Vector3 position, Vector3 direction)
        {
            Movement.SetPosition(position);
            Movement.SetMoveDirection(direction);
        }

        public void ChangeId(EntityId entityId)
        {
            Id = entityId;
        }

        public void ResetHistory()
        {
            Rollback(CurrentStep);
        }
    }
}
