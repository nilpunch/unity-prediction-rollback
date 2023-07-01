using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Character[] _characters;
        [SerializeField] private Camera _camera;

        private void Update()
        {
            if (Input.GetKey(KeyCode.P))
            {
                return;
            }

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

            input = input.normalized;

            foreach (Character character in _characters)
            {
                ICommandTimeline<CharacterMoveCommand> moveCommandTimeline =
                    UnitySimulation.MoveCommandTimelineRegistery.GetTarget(UnitySimulation.CharacterRegistry.GetTargetId(character));
                int lastMoveCommandTick = moveCommandTimeline.GetLatestTickWithCommandBefore(UnitySimulation.CurrentTick);
                int ticksPassedFromLastMoveCommand = UnitySimulation.CurrentTick - lastMoveCommandTick;
                if (ticksPassedFromLastMoveCommand > 10 || !moveCommandTimeline.HasExactCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input)))
                {
                    moveCommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input));
                }

                ICommandTimeline<CharacterShootCommand> shootCommandTimeline =
                    UnitySimulation.ShootCommandTimelineRegistery.GetTarget(UnitySimulation.CharacterRegistry.GetTargetId(character));
                CharacterShootCommand characterShootCommand;
                if (Input.GetMouseButton(0))
                {
                    Vector3 shootDirection = Vector3.ProjectOnPlane(_camera.ScreenToWorldPoint(Input.mousePosition) - character.transform.position, Vector3.forward).normalized;
                    characterShootCommand = new CharacterShootCommand(shootDirection, true);
                }
                else
                {
                    characterShootCommand = new CharacterShootCommand(Vector3.zero, false);
                }

                if (!shootCommandTimeline.HasExactCommand(UnitySimulation.CurrentTick, characterShootCommand))
                {
                    shootCommandTimeline.InsertCommand(UnitySimulation.CurrentTick, characterShootCommand);
                }
            }

            UnitySimulation.WorldTimeline.UpdateEarliestApprovedTick(UnitySimulation.CurrentTick);
        }
    }
}
