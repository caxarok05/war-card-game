using System;

namespace Client.Scripts.Server 
{
    public class FakeServerException : Exception
    {
        public FakeServerException(string message) : base(message)
        {
        }
    }
}