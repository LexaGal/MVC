using System;
using System.Web;
using System.Web.Mvc;
using Algorithm.Authentication;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Algorithm.AOPAttributes
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof (LogAspect))]
    public class AuthentificationAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (HttpContext.Current.Session["client"] != null)
            {
                base.OnEntry(args);
                return;
            }
            throw new UnauthorizedAccessException();
        }
    }
}