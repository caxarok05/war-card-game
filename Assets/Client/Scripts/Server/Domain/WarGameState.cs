using Client.Scripts.Shared;

namespace Client.Scripts.Server
{
    public sealed class WarGameState
    {
        public PlayerState Player { get; } = new();
        public PlayerState Opponent { get; } = new();
        public TableState Table { get; } = new();

        public bool IsStarted { get; private set; }
        public bool IsGameOver { get; private set; }
        public GameOutcomeType GameOutcome { get; private set; } = GameOutcomeType.None;

        public void MarkStarted()
        {
            IsStarted = true;
            IsGameOver = false;
            GameOutcome = GameOutcomeType.None;
        }

        public void Finish(GameOutcomeType outcome)
        {
            IsGameOver = true;
            GameOutcome = outcome;
        }

        public void Reset()
        {
            Player.DrawPile.Clear();
            Player.WonPile.Clear();

            Opponent.DrawPile.Clear();
            Opponent.WonPile.Clear();

            Table.Clear();

            IsStarted = false;
            IsGameOver = false;
            GameOutcome = GameOutcomeType.None;
        }
    }
}