using UnityEngine;

namespace UPR.Samples
{
    public class CharacterShooting : MonoBehaviour
    {
        [SerializeField] private EntityTransform _entityTransform;

        public void Shoot(Vector3 direction)
        {
            var bullet = UnitySimulation.BulletsFactory.Create();
            bullet.Launch(_entityTransform.Position, direction);
        }
    }
}
