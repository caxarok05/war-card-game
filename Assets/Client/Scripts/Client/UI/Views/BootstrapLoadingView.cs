using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Client
{
    public sealed class BootstrapLoadingView : MonoBehaviour
    {
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TMP_Text _statusText;

        public void SetProgress(float progress)
        {
            _progressSlider.value = progress;
        }

        public void SetStatus(string status)
        {
            _statusText.text = status;
        }
    }
}