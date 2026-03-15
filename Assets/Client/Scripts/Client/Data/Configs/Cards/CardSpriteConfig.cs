using System;
using System.Collections.Generic;
using Client.Scripts.Shared;
using UnityEngine;

namespace Client.Scripts.Client
{
    [CreateAssetMenu(fileName = "CardSpriteConfig", menuName = "Configs/Card Sprite Config")]
    public sealed class CardSpriteConfig : ScriptableObject
    {
        [SerializeField] private List<CardSpriteData> _cards = new();

        public Sprite GetCardSprite(Suit suit, Rank rank)
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                CardSpriteData card = _cards[i];
                if (card.Suit == suit && card.Rank == rank)
                {
                    return card.Sprite;
                }
            }

            throw new InvalidOperationException(
                $"Card sprite was not found for Suit: {suit}, Rank: {rank}");
        }
    }
}
