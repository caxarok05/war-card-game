using Client.Scripts.Shared;

namespace Client.Scripts.Client
{
    public sealed class ResolvedTurnData
    {
        public DrawResponse Response { get; }
        public bool IsWar { get; }
        public bool IsGameOver { get; }
        public GameOutcomeType GameOutcome { get; }

        public ResolvedTurnData(
            DrawResponse response,
            bool isWar,
            bool isGameOver,
            GameOutcomeType gameOutcome)
        {
            Response = response;
            IsWar = isWar;
            IsGameOver = isGameOver;
            GameOutcome = gameOutcome;
        }
    }
}