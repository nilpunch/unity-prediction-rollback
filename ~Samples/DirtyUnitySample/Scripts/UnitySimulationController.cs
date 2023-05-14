using UnityEngine;
using UnityEngine.UI;

namespace UPR.Samples
{
    public class UnitySimulationController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private UnitySimulation _unitySimulation;
        [SerializeField] private CharacterController _unityCharacterController;

        private bool _simulationStopped;

        private void Awake()
        {
            _slider.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _simulationStopped = !_simulationStopped;

                if (_simulationStopped)
                {
                    StopSimulation();
                }
                else
                {
                    ContinueSimulationFrom(Mathf.RoundToInt(_slider.value));
                }
            }

            if (_simulationStopped)
            {
                UnitySimulation.TimeTravelMachine.FastForwardToTick(Mathf.RoundToInt(_slider.value));
            }
        }

        private void StopSimulation()
        {
            _unitySimulation.enabled = false;
            _unityCharacterController.enabled = false;

            _slider.gameObject.SetActive(true);
            _slider.maxValue = UnitySimulation.CurrentTick;
            _slider.minValue = 0f;
            _slider.wholeNumbers = true;
            _slider.value = UnitySimulation.CurrentTick;
        }

        private void ContinueSimulationFrom(int tick)
        {
            _unitySimulation.enabled = true;
            _unityCharacterController.enabled = true;
            _slider.gameObject.SetActive(false);

            UnitySimulation.ElapsedTime = (tick + 0.5f) * (UnitySimulation.SimulationSpeed.SecondsPerTick);
            UnitySimulation.CharacterShooting.RemoveAllDownTo(tick);
            UnitySimulation.CharacterMovement.RemoveAllDownTo(tick);
        }
    }
}
