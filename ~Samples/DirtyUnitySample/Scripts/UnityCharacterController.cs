﻿using UnityEngine;

namespace UPR.Samples
{
    public class UnityCharacterController : MonoBehaviour
    {
        [SerializeField] private UnityCharacter _unityCharacter;

        private void Update()
        {
            Vector3 input = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                input += Vector3.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                input += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                input += Vector3.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                input += Vector3.right;
            }

            UnitySimulation.CharacterMovement.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input.normalized), _unityCharacter.Id);

            // if (input.sqrMagnitude < 0.001f)
            // {
            //     input = Vector3.up;
            // }
            //
            // if (Input.GetMouseButtonDown(0))
            // {
            //     UnitySimulation.WorldTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(input.normalized), _unityCharacter.Id);
            // }
        }
    }
}
