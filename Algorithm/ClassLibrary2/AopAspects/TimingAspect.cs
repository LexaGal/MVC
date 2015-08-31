using System;
using System.Diagnostics;
using Aop.AopAspects.Logging;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Aop.AopAspects
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
        }

        public override void OnExit(MethodExecutionArgs args)
        {
             Logger.Log.Info(args.Method + " [Elapsed time: " + _stopWatch.ElapsedMilliseconds + ']'); 
        }
    }
}