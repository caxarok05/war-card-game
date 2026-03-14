using Client.Scripts.Shared;

namespace Client.Scripts.Shared
{
    public sealed class GameOutcomeEvaluator
    {
        public GameOutcomeType Evaluate(WarGameState state)
        {
            if (state.Player.TotalCardCount == 0 && state.Opponent.TotalCardCount == 0)
            {
                return GameOutcomeType.Draw;
            }

            if (state.Player.TotalCardCount == 0)
            {
                return GameOutcomeType.OpponentWon;
            }

            if (state.Opponent.TotalCardCount == 0)
            {
                return GameOutcomeType.PlayerWon;
            }

            return GameOutcomeType.None;
        }
    }
}