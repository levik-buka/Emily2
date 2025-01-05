using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Logger
{
    internal static class LoggerExtensions
    {
        private static readonly int STACK_FRAME_OFFSET = 1;

        internal static ILogger CreateClassLogger(this ILoggerFactory logFactory)
        {
            StackFrame? sf = new StackTrace().GetFrame(STACK_FRAME_OFFSET);

            return logFactory.CreateLogger(sf!.GetMethod()!.DeclaringType!.Name);
        }

        internal static LoggerScope BeginMethodScope(this ILogger logger, params object[] args)
        {
            StackFrame? sf = new StackTrace().GetFrame(STACK_FRAME_OFFSET);

            return new LoggerScope(logger, sf!.GetMethod(), args);
        }

        internal static LoggerScope BeginSubScope(this LoggerScope parentScope, params object[] args)
        {
            StackFrame? sf = new StackTrace().GetFrame(STACK_FRAME_OFFSET);

            return new LoggerScope(parentScope, sf!.GetMethod(), args);
        }

        internal static void LogTraceMethod(this ILogger logger, params object[] args)
        {
            StackFrame? sf = new StackTrace().GetFrame(STACK_FRAME_OFFSET);

            logger.LogTrace("Call {method}", sf!.GetMethod().MethodWithParamsToString(args));
        }

        internal static string MethodWithParamsToString(this MethodBase? mehtod, params object[] args)
        {
            ArgumentNullException.ThrowIfNull(mehtod);

            var strBuilder = new StringBuilder();
            strBuilder.Append(mehtod.ToString());
            strBuilder.ParasParams(mehtod.GetParameters(), args);
            return strBuilder.ToString();
        }

        public static void ParasParams(this StringBuilder strBuilder, ParameterInfo[] paramNames, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
               strBuilder.Append($"{Environment.NewLine}\t{paramNames[i].Name}: {args[i]}");
            }
        }
    }
}
