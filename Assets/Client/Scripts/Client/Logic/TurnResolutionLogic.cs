using Client.Scripts.Shared;

namespace Client.Scripts.Client
{
    public sealed class TurnResolutionLogic
    {
        public ResolvedTurnData Resolve(DrawResponse response)
        {
            bool isGameOver = response.GameOutcome != GameOutcomeType.None;

            return new ResolvedTurnData(
                response,
                response.RoundOutcome,
                response.IsWar,
                isGameOver,
                response.GameOutcome);
        }
    }
}