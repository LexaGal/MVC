using System;
using System.Transactions;
using Aop.AopAspects.Logging;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Aop.AopAspects
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(LogAspect))]
    public class RunInTransactionAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        TransactionScope _transactionScope;
        
        public override void OnEntry(MethodExecutionArgs args)
        {
            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            _transactionScope.Complete();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            Transaction.Current.Rollback();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _transactionScope.Dispose();
        }
    }
}