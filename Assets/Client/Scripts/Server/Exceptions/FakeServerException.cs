using System;

namespace Client.Scripts.Shared 
{
    public class FakeServerException : Exception
    {
        public FakeServerException(string message) : base(message)
        {
        }
    }
}