using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class DrawPileRefillService
    {
        private readonly DeckShuffler _deckShuffler;

        public DrawPileRefillService(DeckShuffler deckShuffler)
        {
            _deckShuffler = deckShuffler;
        }

        public void RefillIfNeeded(PlayerState playerState)
        {
            if (playerState.DrawPile.Count > 0)
            {
                return;
            }

            if (playerState.WonPile.Count == 0)
            {
                return;
            }

            var cardsToShuffle = new List<Card>(playerState.WonPile);
            playerState.WonPile.Clear();

            _deckShuffler.Shuffle(cardsToShuffle);

            for (int i = 0; i < cardsToShuffle.Count; i++)
            {
                playerState.DrawPile.Enqueue(cardsToShuffle[i]);
            }
        }
    }
}