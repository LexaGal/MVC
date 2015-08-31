using System;
using System.Web.Mvc;

namespace Algorithm.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.RequestContext.HttpContext.Response.RedirectPermanent("~/Auth/LogIn");
        }
    }
}