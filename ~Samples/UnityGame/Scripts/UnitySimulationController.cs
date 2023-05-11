using UnityEngine;
using UnityEngine.UI;

namespace UPR.Samples
{
    public class UnitySimulationController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private UnitySimulation _unitySimulation;
        [SerializeField] private UnityCharacterController _unityCharacterController;

        private bool _simulationStopped;

        private void Awake()
        {
            _slider.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_simulationStopped && Input.GetKeyDown(KeyCode.Space))
            {
                _simulationStopped = true;

                _unitySimulation.enabled = false;
                _unityCharacterController.enabled = false;

                _slider.gameObject.SetActive(true);
                _slider.maxValue = _unitySimulation.CurrentTick;
                _slider.minValue = 0f;
                _slider.wholeNumbers = true;
                _slider.value = _unitySimulation.CurrentTick;
            }

            if (_simulationStopped)
                _unitySimulation.WorldTimeline.FastForwardToTick(Mathf.RoundToInt(_slider.value));
        }
    }
}
