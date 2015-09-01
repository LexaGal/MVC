using System;
using Aop.AopAspects.Logging;
using PostSharp.Aspects;

namespace Aop.AopAspects
{
    [Serializable]
    public class ExceptionAspect : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            Logger.Log.Error(args.Method, args.Exception);
        }
    }
}