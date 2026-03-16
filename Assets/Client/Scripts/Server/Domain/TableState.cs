using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Server
{
    public sealed class TableState
    {
        private readonly List<Card> _cards = new();

        public IReadOnlyList<Card> Cards => _cards;

        public void Add(Card card)
        {
            _cards.Add(card);
        }

        public void AddRange(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public List<Card> TakeAll()
        {
            var result = new List<Card>(_cards);
            _cards.Clear();
            return result;
        }

        public void Clear()
        {
            _cards.Clear();
        }
    }
}