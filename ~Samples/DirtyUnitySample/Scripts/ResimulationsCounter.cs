using UnityEngine;
using UnityEngine.UI;

namespace UPR.Samples
{
    public class ResimulationsCounter : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _text;

        private void Update()
        {
            _text.text = $"Resimulate: {(int)_slider.value}";
        }
    }
}
