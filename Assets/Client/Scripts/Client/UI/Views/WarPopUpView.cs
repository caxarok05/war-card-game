using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Client.Scripts.Client
{
    public class WarPopUpView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _textTransform;
        [SerializeField] private GameObject _root;

        [Header("Tween parameters")]
        [SerializeField] private float _scaleFactor = 0.5f;
        [SerializeField] private float _firstFadeDuration = 0.12f;
        [SerializeField] private float _secondFadeDuration = 0.25f;
        
        [SerializeField] private float _firstScaleDuration = 0.18f;
        [SerializeField] private float _secondScaleDuration = 0.12f;
        
        [SerializeField] private float _intervalDuration = 0.35f;
        [SerializeField] private float _scaleValue = 1.25f;
        
        private Tween _sequence;

        private void Awake()
        {
            _root.SetActive(false);
            _canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            _sequence?.Kill();

            _root.SetActive(true);

            _canvasGroup.alpha = 0f;
            _textTransform.localScale = Vector3.one * _scaleFactor;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1f, _firstFadeDuration));
            sequence.Join(_textTransform.DOScale(_scaleValue, _firstScaleDuration).SetEase(Ease.OutExpo));
            sequence.Append(_textTransform.DOScale(1f, _secondScaleDuration).SetEase(Ease.OutQuad));
            sequence.AppendInterval(_intervalDuration);
            sequence.Append(_canvasGroup.DOFade(0f, _secondFadeDuration));
            sequence.OnComplete(() => _root.SetActive(false));

            _sequence = sequence;
        }
    }
}