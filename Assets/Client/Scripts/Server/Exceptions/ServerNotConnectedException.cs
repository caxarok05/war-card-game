namespace Client.Scripts.Server
{
    public sealed class ServerNotConnectedException : FakeServerException
    {
        public ServerNotConnectedException()
            : base("The fake server is not connected.")
        {
        }
    }
}