using System;

namespace Networking.Exceptions
{
    public class FormatterException : Exception
    {
        public FormatterException(string message) : base($"INetworkDataFormatter exception:{message}")
        {

        }
    }
}
