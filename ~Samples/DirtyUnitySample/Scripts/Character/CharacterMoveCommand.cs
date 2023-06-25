using System;
using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public readonly struct CharacterMoveCommand : IEquatable<CharacterMoveCommand>, IDecayedCommand<CharacterMoveCommand>
    {
        public CharacterMoveCommand(Vector3 moveDirection)
        {
            MoveDirection = moveDirection;
        }

        public Vector3 MoveDirection { get; }

        public CharacterMoveCommand FadeOutPercent(float percent)
        {
            return new CharacterMoveCommand(MoveDirection * (1f - percent));
        }

        public bool Equals(CharacterMoveCommand other)
        {
            return MoveDirection.Equals(other.MoveDirection);
        }

        public override bool Equals(object obj)
        {
            return obj is CharacterMoveCommand other && Equals(other);
        }

        public override int GetHashCode()
        {
            return MoveDirection.GetHashCode();
        }
    }
}
