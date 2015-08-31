using System;
using System.Linq;
using System.Web;
using Algorithm.Authentication;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Algorithm.AopAttributes
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
                        throw new AuthenticationException();
                    }
                }
            }
        }
    }
}