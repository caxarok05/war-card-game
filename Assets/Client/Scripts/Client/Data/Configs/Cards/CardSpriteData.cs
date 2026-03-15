using System;
using Client.Scripts.Shared;
using UnityEngine;

namespace Client.Scripts.Client
{
    [Serializable]
    public class CardSpriteData
    {
        [SerializeField] private Suit _suit;
        [SerializeField] private Rank _rank;
        [SerializeField] private Sprite _sprite;

        public Suit Suit => _suit;
        public Rank Rank => _rank;
        public Sprite Sprite => _sprite;
    }
}