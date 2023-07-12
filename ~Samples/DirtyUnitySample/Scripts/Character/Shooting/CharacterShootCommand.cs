using System;
using UnityEngine;

namespace UPR.Samples
{
    public readonly struct CharacterShootCommand : IEquatable<CharacterShootCommand>
    {
        public CharacterShootCommand(Vector3 direction, bool isShooting)
        {
            Direction = direction;
            IsShooting = isShooting;
        }

        public Vector3 Direction { get; }
        public bool IsShooting { get; }

        public bool Equals(CharacterShootCommand other)
        {
            return Direction.Equals(other.Direction) && IsShooting == other.IsShooting;
        }

        public override bool Equals(object obj)
        {
            return obj is CharacterShootCommand other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Direction, IsShooting);
        }
    }
}
