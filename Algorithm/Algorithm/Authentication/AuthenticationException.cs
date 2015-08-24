using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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