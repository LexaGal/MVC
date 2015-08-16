using System;
using System.Diagnostics;
using System.Transactions;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Algorithm.AOPAttributes
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(LogAspect))]
    public class RunInTransactionAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        TransactionScope _transactionScope;
        [NonSerialized]
        Stopwatch _stopWatch;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _stopWatch = Stopwatch.StartNew();
            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            _transactionScope.Complete();
            MvcApplication.Log.Info(args.Method + " [Elapsed time: " + _stopWatch.ElapsedMilliseconds + ']');
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            Transaction.Current.Rollback();
            MvcApplication.Log.Error(args.Method + " [Elapsed time: " + _stopWatch.ElapsedMilliseconds + ']',
                args.Exception);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _transactionScope.Dispose();
        }
    }
}