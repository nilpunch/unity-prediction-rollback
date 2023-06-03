using UnityEngine;

namespace UPR.Samples
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private Camera _camera;

        private void Update()
        {
            Vector3 input = Vector3.zero;

            input = Vector3.down * Mathf.Sin(Time.time) + Vector3.right * Mathf.Cos(Time.time);

            // if (Input.GetKey(KeyCode.W))
            // {
            //     input += Vector3.up;
            // }
            // if (Input.GetKey(KeyCode.A))
            // {
            //     input += Vector3.left;
            // }
            // if (Input.GetKey(KeyCode.S))
            // {
            //     input += Vector3.down;
            // }
            // if (Input.GetKey(KeyCode.D))
            // {
            //     input += Vector3.right;
            // }

            UnitySimulation.CharacterMovement.RemoveAllCommandsAt(UnitySimulation.CurrentTick);

            UnitySimulation.CharacterMovement.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input.normalized), UnitySimulation.CharacterWorld.GetEntityId(_character));

            // if (Input.GetMouseButton(0))
            // if (Input.GetMouseButtonDown(0))
            {
                // Vector3 shootDirection = Vector3.ProjectOnPlane(_camera.ScreenToWorldPoint(Input.mousePosition) - _character.transform.position, Vector3.forward).normalized;
                Vector3 shootDirection = Quaternion.Euler(0f, 0f, 70f) * input.normalized;
                UnitySimulation.CharacterShooting.RemoveAllCommandsAt(UnitySimulation.CurrentTick);
                UnitySimulation.CharacterShooting.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(shootDirection), UnitySimulation.CharacterWorld.GetEntityId(_character));
            }
        }
    }
}
