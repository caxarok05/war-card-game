using System.Collections.Generic;
using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class PlayerState
    {
        private readonly Queue<Card> _drawPile = new();
        private readonly List<Card> _wonPile = new();

        public Queue<Card> DrawPile => _drawPile;
        public List<Card> WonPile => _wonPile;

        public int TotalCardCount => _drawPile.Count + _wonPile.Count;
    }
}