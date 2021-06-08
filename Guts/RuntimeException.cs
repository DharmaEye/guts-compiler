using System;

namespace Guts
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message)
        {

        }
    }
}