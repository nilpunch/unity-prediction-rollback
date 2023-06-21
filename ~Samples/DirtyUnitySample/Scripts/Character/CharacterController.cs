﻿using UnityEngine;
using UPR.Networking;

namespace UPR.Samples
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Character[] _characters;
        [SerializeField] private Camera _camera;

        private void Update()
        {
            Vector3 input = Vector3.zero;

            // input = Vector3.down * Mathf.Sin(UnitySimulation.ElapsedTime) + Vector3.right * Mathf.Cos(UnitySimulation.ElapsedTime);

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

            foreach (Character character in _characters)
            {
                ICommandTarget<CharacterMoveCommand> moveCommandTarget = character;
                moveCommandTarget.CommandTimeline.RemoveAllCommandsDownTo(UnitySimulation.CurrentTick);
                moveCommandTarget.CommandTimeline.RemoveCommand(UnitySimulation.CurrentTick);
                moveCommandTarget.CommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input.normalized));

                ICommandTarget<CharacterShootCommand> shootCommandTarget = character;
                shootCommandTarget.CommandTimeline.RemoveAllCommandsDownTo(UnitySimulation.CurrentTick);
                shootCommandTarget.CommandTimeline.RemoveCommand(UnitySimulation.CurrentTick);
                if (Input.GetMouseButton(0))
                {
                    Vector3 shootDirection = Vector3.ProjectOnPlane(_camera.ScreenToWorldPoint(Input.mousePosition) - character.transform.position, Vector3.forward).normalized;
                    shootCommandTarget.CommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(shootDirection, true));
                }
                else
                {
                    shootCommandTarget.CommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(Vector3.zero, false));
                }
            }

            UnitySimulation.WorldTimeline.UpdateEarliestApprovedTick(UnitySimulation.CurrentTick);
        }
    }
}
