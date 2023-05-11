using UnityEngine;

namespace UPR.Samples
{
    public class UnityCharacterController : MonoBehaviour
    {
        [SerializeField] private UnitySimulation _unitySimulation;
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

            _unitySimulation.WorldTimeline.InsertCommand(_unitySimulation.CurrentTick + 1, new CharacterMoveCommand(input.normalized), _unityCharacter.Id);
        }
    }
}
