using System;
using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class DeckShuffler
    {
        private readonly Random _random;

        public DeckShuffler()
        {
            _random = new Random();
        }

        public void Shuffle(List<Card> cards)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int swapIndex = _random.Next(i + 1);
                (cards[i], cards[swapIndex]) = (cards[swapIndex], cards[i]);
            }
        }
    }
}