using UnityEngine;

namespace UPR.Samples
{
    public class DeathRaycast : MonoBehaviour, ISimulation
    {
        [SerializeField] private Vector3 _dimensions;

        private readonly Collider[] _castResults = new Collider[100];

        public void StepForward()
        {
            Physics.SyncTransforms();

            int overlaps = Physics.OverlapBoxNonAlloc(transform.position, _dimensions / 2f, _castResults);

            for (int i = 0; i < overlaps; i++)
            {
                var result = _castResults[i];

                if (result.TryGetComponent(out Character character) && UnitySimulation.CharacterWorld.IsAlive(character.Id))
                {
                    UnitySimulation.CharacterWorld.KillEntity(character.Id);
                }

                if (result.TryGetComponent(out Bullet bullet) && UnitySimulation.BulletsWorld.IsAlive(bullet.Id))
                {
                    UnitySimulation.BulletsWorld.KillEntity(bullet.Id);
                }
            }
        }
    }
}
