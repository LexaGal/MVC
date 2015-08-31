using System;
using PostSharp.Aspects;

namespace Algorithm.AopAttributes
{
    [Serializable]
    public class ExceptionAspect : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            MvcApplication.Log.Error(args.Method, args.Exception);
        }
    }
}