using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Logger
{
    internal class LoggerScope : IDisposable
    {
        private ILogger _logger;
        private EventId _eventId;

        /// <summary>
        /// Creating scope for logging in public/internal methods
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="scope"></param>
        /// <param name="args"></param>
        public LoggerScope(ILogger logger, MethodBase scope, params object[] args)
        {
            _logger = logger;
            _eventId = new EventId(1, scope.Name);

            _logger.LogTrace("Enter {scope}", scope.MethodWithParamsToString(args));
        }

        /// <summary>
        /// Creates sub-scope for logging in protected/private methods
        /// </summary>
        /// <param name="parentScope"></param>
        /// <param name="scope"></param>
        /// <param name="args"></param>
        public LoggerScope(LoggerScope parentScope, MethodBase scope, params object[] args)
        {
            _logger = parentScope._logger;
            _eventId = new EventId(parentScope._eventId.Id + 1, scope.Name);

            parentScope.LogTrace("Enter {scope}", scope.MethodWithParamsToString(args));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_logger != null)
            {
                if (disposing)
                {
                    _logger.LogTrace(_eventId, "Exit");
                }
            }
        }

        public void LogTrace(string? message, params object?[] args)
        {
            _logger.Log(LogLevel.Trace, _eventId, message, args);
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LoggerScope()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
