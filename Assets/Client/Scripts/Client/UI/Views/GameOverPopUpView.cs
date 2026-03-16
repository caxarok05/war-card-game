using System;
using Client.Scripts.Shared;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Client
{
    public class GameOverPopUpView : MonoBehaviour
    {
        public Action NextButtonClicked;

        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private GameObject _popUpRoot;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Tween parameters")]
        [SerializeField] private float _fadeDuration = 0.2f;
        
        private Tween _showTween;

        private void Awake()
        {
            _popUpRoot.SetActive(false);
            _canvasGroup.alpha = 0f;
        }

        private void Start()
        {
            _nextButton.onClick.AddListener(ClickNextButton);
        }

        private void OnDestroy()
        {
            _nextButton.onClick.RemoveListener(ClickNextButton);
        }

        public void ShowPopUp(string resultText)
        {
            _showTween?.Kill();

            _resultText.text = resultText;
            _popUpRoot.SetActive(true);
            _canvasGroup.alpha = 0f;

            _showTween = _canvasGroup.DOFade(1f, _fadeDuration);
        }

        private void ClickNextButton() => NextButtonClicked?.Invoke();
    }
}