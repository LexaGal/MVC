using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;

namespace Algorithm.AOPAttributes
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