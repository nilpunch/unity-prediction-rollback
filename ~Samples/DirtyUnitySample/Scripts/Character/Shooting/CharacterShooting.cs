using UnityEngine;

namespace UPR.Samples
{
    public class CharacterShooting : MonoBehaviour
    {
        public void Shoot(Vector3 direction)
        {
            var bullet = UnitySimulation.BulletsFactory.Create();
            bullet.Launch(transform.position, direction);
        }
    }
}
