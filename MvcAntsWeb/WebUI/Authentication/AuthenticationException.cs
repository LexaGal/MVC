using System;

namespace Algorithm.Authentication
{
    public class AuthenticationException : ApplicationException
    {
        public override string Message
        {
            get { return (new UnauthorizedAccessException()).Message; }
        }
    }
}