namespace Client.Scripts.Shared
{
    public sealed class FakeServerTimeoutException : FakeServerException
    {
        public FakeServerTimeoutException()
            : base("The fake server request timed out.")
        {
        }
    }
}