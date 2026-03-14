using System.Collections.Generic;

namespace Client.Scripts.Shared
{
    public sealed class DrawResponse
    {
        public IReadOnlyList<CardRevealData> RevealedCards { get; }
        public RoundOutcomeType RoundOutcome { get; }
        public GameOutcomeType GameOutcome { get; }
        public bool IsWar { get; }
        public int PlayerTotalCardCount { get; }
        public int OpponentTotalCardCount { get; }

        public DrawResponse(
            IReadOnlyList<CardRevealData> revealedCards,
            RoundOutcomeType roundOutcome,
            GameOutcomeType gameOutcome,
            bool isWar,
            int playerTotalCardCount,
            int opponentTotalCardCount)
        {
            RevealedCards = revealedCards;
            RoundOutcome = roundOutcome;
            GameOutcome = gameOutcome;
            IsWar = isWar;
            PlayerTotalCardCount = playerTotalCardCount;
            OpponentTotalCardCount = opponentTotalCardCount;
        }
    }
}