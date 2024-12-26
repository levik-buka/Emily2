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

        /// <summary>
        /// Global logger factory saved here
        /// </summary>
        internal static ILoggerFactory LoggerFactory { get; set; }

        internal static ILogger CreateClassLogger()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(STACK_FRAME_OFFSET);

            return LoggerFactory.CreateLogger(sf.GetMethod().DeclaringType.Name);
        }

        internal static LoggerScope BeginMethodScope(this ILogger logger, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(STACK_FRAME_OFFSET);

            return new LoggerScope(logger, sf.GetMethod(), args);
        }

        internal static void TraceMethod(this ILogger logger, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(STACK_FRAME_OFFSET);

            logger.LogTrace("Called {method}", sf.GetMethod().MethodWithParamsToString(args));
        }

        internal static string MethodWithParamsToString(this MethodBase mehtod, params object[] args)
        {
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
