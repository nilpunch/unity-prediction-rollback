using UnityEngine;

namespace UPR.Samples
{
    public struct CharacterShootCommand
    {
        public CharacterShootCommand(Vector3 direction)
        {
            Direction = direction;
        }

        public Vector3 Direction { get; }
    }
}
