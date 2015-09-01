using System;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using Aop.AopAspects.Logging;
using Entities.DatabaseModels;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Aop.AopAspects
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof (LogAspect))]
    public class AuthentificationAspect : OnMethodBoundaryAspect
    {
        private readonly bool _needAuth;

        public AuthentificationAspect(bool f = true)
        {
            _needAuth = f;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Method.GetParameters().Any() &&
                args.Method.GetParameters()[0].ParameterType.Name != typeof (User).Name)
            {
                if (!args.Method.IsConstructor)
                {
                    if (_needAuth)
                    {
                        if (HttpContext.Current.Session["client"] != null)
                        {
                            return;
                        }
                        HttpContext.Current.Response.RedirectPermanent("/Auth/LogIn");
                    }
                }
            }
        }
    }
}