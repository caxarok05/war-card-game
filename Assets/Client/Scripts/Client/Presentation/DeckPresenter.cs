using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class DeckPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        
        [SerializeField] private TMP_Text _cardAmountText;
        [SerializeField] private List<GameObject> _deckCards;
        private int _count;

        public Vector3 GetSpawnPoint()
        {
            return _spawnPoint.position;
        }

        public Quaternion GetSpawnRotation()
        {
            return _spawnPoint.rotation;
        }

        public void SetCount(int count)
        {
            _count = count;
            _cardAmountText.text = count.ToString();
            UpdateDeckVisual();
            
        }
        
        private void UpdateDeckVisual()
        {
            int visibleCards = GetVisibleDeckCardCount(_count);

            for (int i = 0; i < _deckCards.Count; i++)
            {
                _deckCards[i].SetActive(i < visibleCards);
            }
        }

        private int GetVisibleDeckCardCount(int count)
        {
            if (count <= 0)
                return 0;

            if (count <= 5)
                return Mathf.Min(1, _deckCards.Count);

            if (count <= 10)
                return Mathf.Min(2, _deckCards.Count);

            if (count <= 20)
                return Mathf.Min(3, _deckCards.Count);

            if (count <= 35)
                return Mathf.Min(4, _deckCards.Count);

            return Mathf.Min(5, _deckCards.Count);
        }
    }
}