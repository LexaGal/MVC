using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Text;
using System.Web.Mvc;
using Aop.AopAspects.Caching;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Aop.AopAspects.Logging
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(CacheableResultAspect))]
    public class LogAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        Stopwatch _stopWatch;
        [NonSerialized]
        StringBuilder _stringBuilder;

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Method.Name == "OnException")
            {
                OnException(args);
                return;
            }

            if (!args.Method.IsConstructor)
            {
                _stopWatch = Stopwatch.StartNew();
                _stringBuilder = new StringBuilder();

                for (int index = 0; index < args.Arguments.Count; index++)
                {
                    var arg = args.Arguments.GetArgument(index);
                    _stringBuilder.AppendFormat("({0}: {1}) ", args.Method.GetParameters()[index].ParameterType.Name,
                        TypeConverter.GetStringView(args.Method.GetParameters()[index].ParameterType.Name, arg));
                }
                Logger.Log.InfoFormat("{0} [{1}]", args.Method.Name, _stringBuilder);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (args.Method.Name == "Edit" || args.Method.Name == "Add" || args.Method.Name == "Delete")
            {
                MethodResultCache.ClearAllCachedResults();
            }

            if (!args.Method.IsConstructor && args.ReturnValue.GetType().Name != typeof(RedirectResult).Name)
            {
                if (args.ReturnValue != null)
                {
                    if (args.ReturnValue.GetType().IsGenericType)
                    {
                        string stringView = TypeConverter.GetStringView(args.ReturnValue.GetType().FullName, args.ReturnValue);
                        Logger.Log.Info(string.Format("{0} {1} [Elapsed time: {2}]", args.Method.Name, stringView, _stopWatch.ElapsedMilliseconds));
                        return;
                    }
                    Logger.Log.Info(string.Format("{0} {1} [Elapsed time: {2}]", args.Method.Name, args.ReturnValue, _stopWatch.ElapsedMilliseconds));
                }
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            if (args.Exception != null)
            {
                Logger.Log.Error(args.Exception.Message, args.Exception);
                return;
            }
            var e = new AuthenticationException();
            Logger.Log.Error(e.Message, e);
        }
    }
}