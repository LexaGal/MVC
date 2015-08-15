using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;

namespace Algorithm.AOPAttributes
{
    [Serializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            MvcApplication.Log.Info(args.Method);
            base.OnExit(args);
        }
    }
}