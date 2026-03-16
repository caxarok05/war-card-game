namespace Client.Scripts.Server
{
    public sealed class GameAlreadyFinishedException : FakeServerException
    {
        public GameAlreadyFinishedException()
            : base("The game is already finished.")
        {
        }
    }
}