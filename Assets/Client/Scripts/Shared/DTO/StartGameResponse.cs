namespace Client.Scripts.Shared
{
    public sealed class StartGameResponse
    {
        public int PlayerDeckCount { get; }
        public int OpponentDeckCount { get; }

        public StartGameResponse(int playerDeckCount, int opponentDeckCount)
        {
            PlayerDeckCount = playerDeckCount;
            OpponentDeckCount = opponentDeckCount;
        }
    }
}