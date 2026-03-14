namespace Client.Scripts.Shared
{
    public sealed class FakeServerNetworkException : FakeServerException
    {
        public FakeServerNetworkException()
            : base("A fake network error occurred while processing the request.")
        {
        }
    }
}