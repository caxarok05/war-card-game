using DG.Tweening;
using UnityEngine;

namespace Client.Scripts.Client
{
    public class GameStartOverlayView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _root;

        [Header("Tween parameters")]
        [SerializeField] private float _fadeDuration = 0.35f;
        
        private Tween _fadeTween;

        private void Awake()
        {
            _root.SetActive(false);
            _canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            _fadeTween?.Kill();

            _root.SetActive(true);
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(0f, _fadeDuration)
                .OnComplete(() => _root.SetActive(false));
        }
    }
}