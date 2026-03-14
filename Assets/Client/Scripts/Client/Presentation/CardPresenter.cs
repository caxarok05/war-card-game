using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class CardPresenter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _frontRenderer;
        [SerializeField] private GameObject _frontRoot;
        [SerializeField] private GameObject _backRoot;

        public void ShowFront(Card card)
        {
            _frontRoot.SetActive(true);
            _backRoot.SetActive(false);

            // TODO: resolve sprite by card rank/suit through config provider or card sprite service.
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
            {
                _frontRenderer.sortingOrder = sortingOrder;
            }
        }

        public async UniTask MoveToAsync(
            Vector3 targetPosition,
            Quaternion targetRotation,
            CancellationToken cancellationToken)
        {
            // TODO: replace with tween based animation using config provider timings.
            transform.SetPositionAndRotation(targetPosition, targetRotation);
            await UniTask.Yield(cancellationToken);
        }

        public void ResetState()
        {
            gameObject.SetActive(false);
        }
    }
}