using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class WarGameInitializer
    {
        private readonly DeckBuilder _deckBuilder;
        private readonly DeckShuffler _deckShuffler;

        public WarGameInitializer(DeckBuilder deckBuilder, DeckShuffler deckShuffler)
        {
            _deckBuilder = deckBuilder;
            _deckShuffler = deckShuffler;
        }

        public StartGameResponse Initialize(WarGameState state)
        {
            state.Reset();

            List<Card> deck = _deckBuilder.BuildStandardDeck();
            _deckShuffler.Shuffle(deck);

            DealCards(deck, state);
            state.MarkStarted();

            return new StartGameResponse(
                state.Player.TotalCardCount,
                state.Opponent.TotalCardCount);
        }

        private void DealCards(List<Card> deck, WarGameState state)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                if (i % 2 == 0)
                {
                    state.Player.DrawPile.Enqueue(deck[i]);
                }
                else
                {
                    state.Opponent.DrawPile.Enqueue(deck[i]);
                }
            }
        }
    }
}