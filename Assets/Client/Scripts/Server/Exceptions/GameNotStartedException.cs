namespace Client.Scripts.Shared
{
    public sealed class GameNotStartedException : FakeServerException
    {
        public GameNotStartedException()
            : base("The game has not been started yet.")
        {
        }
    }
}