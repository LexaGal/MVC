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
            Console.WriteLine("[{0}] took {1}ms to execute", new StackTrace().GetFrame(1).GetMethod().Name, _stopWatch.ElapsedMilliseconds);
            base.OnExit(args);
        }
    }
}