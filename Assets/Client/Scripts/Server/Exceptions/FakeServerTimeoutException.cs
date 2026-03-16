namespace Client.Scripts.Server
{
    public sealed class FakeServerTimeoutException : FakeServerException
    {
        public FakeServerTimeoutException()
            : base("The fake server request timed out.")
        {
        }
    }
}