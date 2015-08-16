using System;
using System.Diagnostics;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Algorithm.AOPAttributes
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public class TimingAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        Stopwatch _stopWatch;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _stopWatch = Stopwatch.StartNew();
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            MvcApplication.Log.Info(args.Method + " [Elapsed time: " + _stopWatch.ElapsedMilliseconds + ']'); 
            base.OnExit(args);
        }
    }
}