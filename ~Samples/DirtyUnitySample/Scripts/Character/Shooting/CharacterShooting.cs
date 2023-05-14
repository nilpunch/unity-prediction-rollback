using UnityEngine;

namespace UPR.Samples
{
    public class CharacterShooting : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _characterMovement;

        public void Shoot(Vector3 direction)
        {
            var bullet = UnitySimulation.BulletsFactory.Create();
            bullet.Launch(_characterMovement.Position, direction);
        }
    }
}
