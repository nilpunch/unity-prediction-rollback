using UnityEngine;
using UnityEngine.UI;

namespace UPR.Samples
{
    public class EntitiesCounter : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private UnitySimulation _unitySimulation;

        private void Update()
        {
            _text.text = $"Entities: {_unitySimulation.ActiveEntitiesCount()}";
        }
    }
}
