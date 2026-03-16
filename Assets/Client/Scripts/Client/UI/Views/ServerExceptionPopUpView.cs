using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Client
{
    public class ServerExceptionPopUpView : MonoBehaviour
    {
        public event Action OnCloseClicked;
        
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _panelTransform;
        [SerializeField] private GameObject _popUpRoot;
        [SerializeField] private Button _closeButton;

        [Header("Tween parameters")]
        [SerializeField] private float _scaleFactor = 0.9f;
        [SerializeField] private float _fadeShowDuration = 0.2f;
        [SerializeField] private float _fadeHideDuration = 0.15f;
        [SerializeField] private float _hideEndValue = 0.95f;
        
        private Tween _showTween;
        private Tween _hideTween;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
            
            _popUpRoot.SetActive(false);
            _canvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
        }
        
        public void Show(string message)
        {
            _showTween?.Kill();
            _hideTween?.Kill();

            _messageText.text = message;
            _popUpRoot.SetActive(true);

            _canvasGroup.alpha = 0f;
            _panelTransform.localScale = Vector3.one * _scaleFactor;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1f, _fadeShowDuration));
            sequence.Join(_panelTransform.DOScale(1f, _fadeShowDuration).SetEase(Ease.OutBack));

            _showTween = sequence;
        }

        public void Hide()
        {
            _showTween?.Kill();
            _hideTween?.Kill();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0f, _fadeHideDuration));
            sequence.Join(_panelTransform.DOScale(_hideEndValue, _fadeHideDuration));
            sequence.OnComplete(() => _popUpRoot.SetActive(false));

            _hideTween = sequence;
        }
        
        private void Close() => OnCloseClicked?.Invoke();
    }
}