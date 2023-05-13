using Tools;
using UnityEngine;

namespace UPR.Samples
{
    public class Bullet : UnityEntity
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
            _renderer.enabled = UnitySimulation.BulletWorld.IsAlive(Id);
        }
    }
}
