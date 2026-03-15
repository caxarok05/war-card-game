using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class CardPresenter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _frontRenderer;
        [SerializeField] private SpriteRenderer _backRenderer;
        [SerializeField] private GameObject _frontRoot;
        [SerializeField] private GameObject _backRoot;

        private Tween _activeTween;
        
        public void ShowFront(Sprite sprite)
        {
            _frontRoot.SetActive(true);
            _backRoot.SetActive(false);

            _frontRenderer.sprite = sprite;
        }

        public void ShowBack()
        {
            _frontRoot.SetActive(false);
            _backRoot.SetActive(true);
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        public void SetSortingOrder(int sortingOrder)
        {
            if (_frontRenderer != null)
                _frontRenderer.sortingOrder = sortingOrder;

            if (_backRenderer != null)
                _backRenderer.sortingOrder = sortingOrder;
        }

        public async UniTask MoveToAsync(
            Vector3 targetPosition,
            Quaternion targetRotation,
            float duration,
            CancellationToken cancellationToken)
        {
            KillActiveTween();

            Sequence sequence = DOTween.Sequence();

            sequence.Join(transform.DOMove(targetPosition, duration).SetEase(Ease.InOutQuad));
            sequence.Join(transform.DORotateQuaternion(targetRotation, duration).SetEase(Ease.InOutQuad));

            _activeTween = sequence;

            await sequence.ToUniTask(cancellationToken: cancellationToken);

            transform.SetPositionAndRotation(targetPosition, targetRotation);
            _activeTween = null;
        }

        public async UniTask FlyToDeckAsync(
            Vector3 targetPosition,
            Quaternion targetRotation,
            float duration,
            float arcHeight,
            float extraZRotation,
            CancellationToken cancellationToken)
        {
            KillActiveTween();

            Vector3 startPosition = transform.position;
            Vector3 middlePosition = Vector3.Lerp(startPosition, targetPosition, 0.5f);
            middlePosition.y += arcHeight;

            Vector3[] path =
            {
                startPosition,
                middlePosition,
                targetPosition
            };

            Vector3 endEuler = targetRotation.eulerAngles;
            Vector3 rotatedEuler = new Vector3(endEuler.x, endEuler.y, endEuler.z + extraZRotation);

            Sequence sequence = DOTween.Sequence();

            sequence.Join(
                transform.DOPath(path, duration, PathType.CatmullRom)
                    .SetEase(Ease.InOutQuad));

            sequence.Join(
                transform.DORotate(rotatedEuler, duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutQuad));

            _activeTween = sequence;

            await sequence.ToUniTask(cancellationToken: cancellationToken);

            transform.SetPositionAndRotation(targetPosition, targetRotation);
            _activeTween = null;
        }

        public void ResetState()
        {
            KillActiveTween();
            gameObject.SetActive(false);
        }

        private void KillActiveTween()
        {
            if (_activeTween != null && _activeTween.IsActive())
            {
                _activeTween.Kill();
                _activeTween = null;
            }
        }

        private void OnDisable()
        {
            KillActiveTween();
        }
    }
}