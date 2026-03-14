namespace Client.Scripts.Shared
{
    public sealed class ServerNotConnectedException : FakeServerException
    {
        public ServerNotConnectedException()
            : base("The fake server is not connected.")
        {
        }
    }
}