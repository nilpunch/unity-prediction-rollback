using UnityEngine;

namespace UPR.Samples
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Character _character;

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

            UnitySimulation.CharacterMovement.RemoveAllCommands(UnitySimulation.CurrentTick);
            UnitySimulation.CharacterMovement.InsertCommand(UnitySimulation.CurrentTick, new CharacterMoveCommand(input.normalized), _character.Id);

            if (input.sqrMagnitude < 0.001f)
            {
                input = Vector3.up;
            }

            if (Input.GetMouseButtonDown(0))
            {
                UnitySimulation.CharacterShooting.RemoveAllCommands(UnitySimulation.CurrentTick);
                UnitySimulation.CharacterShooting.InsertCommand(UnitySimulation.CurrentTick, new CharacterShootCommand(input.normalized), _character.Id);
            }
        }
    }
}
