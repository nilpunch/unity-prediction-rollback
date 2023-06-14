using UnityEngine;
using UPR.Networking;

namespace UPR.Samples
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Character _character;
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

            _character.MoveCommandTimeline.RemoveAllCommandsDownTo(UnitySimulation.CurrentTick);
            _character.MoveCommandTimeline.RemoveCommand(UnitySimulation.CurrentTick);
            _character.MoveCommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input.normalized));

            _character.ShootCommandTimeline.RemoveAllCommandsDownTo(UnitySimulation.CurrentTick);
            _character.ShootCommandTimeline.RemoveCommand(UnitySimulation.CurrentTick);
            if (Input.GetMouseButton(0))
            {
                Vector3 shootDirection = Vector3.ProjectOnPlane(_camera.ScreenToWorldPoint(Input.mousePosition) - _character.transform.position, Vector3.forward).normalized;
                _character.ShootCommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(shootDirection, true));
            }
            else
            {
                _character.ShootCommandTimeline.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(Vector3.zero, false));
            }

            UnitySimulation.WorldTimeline.UpdateEarliestApprovedTick(UnitySimulation.CurrentTick);
        }
    }
}
