using UnityEngine;

namespace UPR.Samples
{
    public struct CharacterMoveCommand
    {
        public CharacterMoveCommand(Vector3 moveDirection)
        {
            MoveDirection = moveDirection;
        }

        public Vector3 MoveDirection { get; }
    }
}