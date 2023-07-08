using UnityEngine;
using UnityEngine.UI;

namespace UPR.Samples
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private int[] _frameRateSamples;
        private readonly int _averageFromAmount = 30;

        private int _averageCounter;
        private int _currentAveraged;

        void Awake()
        {
            _frameRateSamples = new int[_averageFromAmount];
        }
        void Update()
        {
            var currentFrame = (int)Mathf.Round(1f / Time.smoothDeltaTime);
            _frameRateSamples[_averageCounter] = Mathf.Max(currentFrame, 0);

            var average = 0f;
            foreach (var frameRate in _frameRateSamples)
            {
                average += frameRate;
            }
            _currentAveraged = (int)Mathf.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;

            _text.text = $"{_currentAveraged.ToString()} FPS";
        }
    }
}
