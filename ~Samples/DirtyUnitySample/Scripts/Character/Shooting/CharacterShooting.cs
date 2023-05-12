using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace UPR.Samples
{
    public class CharacterShooting : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private CharacterMovement _characterMovement;

        private IPoolFactory<Bullet> _factory;
        private Dictionary<EntityId, Bullet> _createdBullets = new Dictionary<EntityId, Bullet>();

        private void Awake()
        {
            _factory = new PrefabPoolFactory<Bullet>(_bulletPrefab);
        }

        public void Shoot(Vector3 direction)
        {
            var bulletId = UnitySimulation.IdGenerator.Generate();

            Bullet bullet = default;
            if (_createdBullets.ContainsKey(bulletId))
            {
                bullet = _createdBullets[bulletId];
            }
            else
            {
                bullet = _factory.Create();
                _createdBullets.Add(bulletId, bullet);
            }

            var bulletMovementMemory = new CharacterMovementMemory()
            {
                MoveDirection = direction, Position = _characterMovement.Save().Position
            };
            bullet.Movement.Load(bulletMovementMemory);
            bullet.Id = bulletId;
            UnitySimulation.EntityWorld.RegisterEntity(bullet);
        }
    }
}
