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

            UnitySimulation.CharacterMovement.RemoveAllDownTo(UnitySimulation.CurrentTick);

            if (!_character.Lifetime.IsAlive)
            {
                return;
            }

            UnitySimulation.CharacterMovement.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input.normalized), UnitySimulation.CharacterWorld.GetEntityId(_character));

            if (input.sqrMagnitude < 0.001f)
            {
                input = Vector3.up;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 shootDirection = Vector3.ProjectOnPlane(_camera.ScreenToWorldPoint(Input.mousePosition) - _character.EntityTransform.Position, Vector3.forward).normalized;
                UnitySimulation.CharacterShooting.RemoveAllDownTo(UnitySimulation.CurrentTick);
                UnitySimulation.CharacterShooting.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(shootDirection), UnitySimulation.CharacterWorld.GetEntityId(_character));
            }
        }
    }
}
