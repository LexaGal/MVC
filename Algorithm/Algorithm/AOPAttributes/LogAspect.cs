using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using Algorithm.Converter;
using Algorithm.DomainModels;
using PostSharp.Aspects;
using WebGrease.Css.Extensions;

namespace Algorithm.AOPAttributes
{
    [Serializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        Stopwatch _stopWatch;
        [NonSerialized]
        StringBuilder _stringBuilder;

        public override void OnEntry(MethodExecutionArgs args)
        {
            TypeConverter typeConverter = new TypeConverter();
            _stopWatch = Stopwatch.StartNew();
            _stringBuilder = new StringBuilder().AppendFormat("{0} [", args.Method.Name);

            for (int index = 0; index < args.Arguments.Count; index++)
            {
                var arg = args.Arguments.GetArgument(index);
                _stringBuilder.AppendFormat("({0}: {1}) ", args.Method.GetParameters()[index].ParameterType.Name,
                    typeConverter.GetStringView(args.Method.GetParameters()[index].ParameterType.Name, arg));
            }
            MvcApplication.Log.Info(_stringBuilder.Append(']').ToString());
            base.OnEntry(args);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            TypeConverter typeConverter = new TypeConverter();
            _stringBuilder = new StringBuilder().AppendFormat("{0} [", args.ReturnValue);

            if (args.ReturnValue.GetType().IsGenericType)
            {
                var objects = args.ReturnValue as List<ResultInfo>;

                for (int index = 0; index < objects.Count; index++)
                {
                    var arg = objects[index];
                    _stringBuilder.AppendFormat("({0}: {1}) ", index + 1,
                        typeConverter.GetStringView("ResultInfo", arg));
                }
                MvcApplication.Log.Info(_stringBuilder.Append(']') + " [Elapsed time: " + _stopWatch.ElapsedMilliseconds + ']');
                return;
            }
            MvcApplication.Log.Info(args.ReturnValue + " [Elapsed time: " + _stopWatch.ElapsedMilliseconds + ']');
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            MvcApplication.Log.Error(args.Exception.Message, args.Exception);
        }
    }
}