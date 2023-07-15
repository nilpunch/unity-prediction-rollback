using UnityEngine;
using UnityEngine.UI;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class UnitySimulationController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _eraseSomeHistoryButton;
        [SerializeField] private UnitySimulation _unitySimulation;
        [SerializeField] private CharacterController _unityCharacterController;

        private bool _simulationStopped;

        private void Awake()
        {
            _slider.gameObject.SetActive(false);
            _pauseButton.onClick.AddListener(() =>
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
            });
            _eraseSomeHistoryButton.onClick.AddListener(() =>
            {
                _unitySimulation.ForgetFromBegin(60);
            });
        }

        private void Update()
        {
            if (_simulationStopped)
            {
                UnitySimulation.WorldTimeline.FastForwardToTick(Mathf.RoundToInt(_slider.value));
            }
        }

        private void StopSimulation()
        {
            _unitySimulation.enabled = false;
            _unityCharacterController.enabled = false;

            _slider.gameObject.SetActive(true);
            _slider.maxValue = UnitySimulation.CurrentTick;
            _slider.minValue = UnitySimulation.CurrentTick - UnitySimulation.RebaseCounter.StepsSaved;
            _slider.wholeNumbers = true;
            _slider.value = UnitySimulation.CurrentTick;
        }

        private void ContinueSimulationFrom(int tick)
        {
            _unitySimulation.enabled = true;
            _unityCharacterController.enabled = true;
            _slider.gameObject.SetActive(false);

            UnitySimulation.ElapsedTime = (tick + 0.5f) * (UnitySimulation.SimulationSpeed.SecondsPerTick);
            UnitySimulation.WorldTimeline.UpdateEarliestApprovedTick(tick);
            foreach (var commandTimeline in UnitySimulation.MoveCommandTimelineRegistery.Entries)
                commandTimeline.RemoveAllCommandsDownTo(tick);
            foreach (var commandTimeline in UnitySimulation.ShootCommandTimelineRegistery.Entries)
                commandTimeline.RemoveAllCommandsDownTo(tick);
        }
    }
}
