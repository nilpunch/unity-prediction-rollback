using UnityEngine;

namespace UPR.Samples
{
    public struct CharacterShootCommand
    {
        public CharacterShootCommand(Vector3 direction, bool isShooting)
        {
            Direction = direction;
            IsShooting = isShooting;
        }

        public Vector3 Direction { get; }
        public bool IsShooting { get; }
    }
}
