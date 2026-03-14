using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class DeckBuilder
    {
        public List<Card> BuildStandardDeck()
        {
            var deck = new List<Card>(52);

            foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
                {
                    deck.Add(new Card(rank, suit));
                }
            }

            return deck;
        }
    }
}