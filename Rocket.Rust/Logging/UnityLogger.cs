using System;

using Rocket.API.Logging;

using Unity = UnityEngine.Debug;

namespace Rocket.Rust.Logging
{
    public class UnityLogger : ILogger
    {
        private readonly string prefix = "[ROCKET, {0}]";
        
        private readonly string infoPrefix = "INFO";
        private readonly string warnPrefix = "WARN";
        private readonly string errorPrefix = "ERROR";
        private readonly string fatalPrefix = "FATAL";

        public bool IsTraceEnabled => false;
        public bool IsDebugEnabled => false;

        public bool IsInfoEnabled => true;
        public bool IsWarnEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;

        public void Error(string message, params object[] arguments)
        {
            Unity.LogError($"{string.Format(prefix, errorPrefix)} {string.Format(message, arguments)}");
        }

        public void Error(string message, Exception exception, params object[] arguments)
        {
            Unity.LogError($"{string.Format(prefix, errorPrefix)} {string.Format(message, arguments)}");
            Unity.LogException(exception);
        }

        public void Fatal(string message, params object[] arguments)
        {
            Unity.LogError($"{string.Format(prefix, fatalPrefix)} {string.Format(message, arguments)}");
        }

        public void Fatal(string message, Exception exception, params object[] arguments)
        {
            Unity.LogError($"{string.Format(prefix, fatalPrefix)} {string.Format(message, arguments)}");
            Unity.LogException(exception);
        }

        public void Info(string message, params object[] arguments)
        {
            Unity.Log($"{string.Format(prefix, infoPrefix)} {string.Format(message, arguments)}");
        }

        public void Info(string message, Exception exception, params object[] arguments)
        {
            Unity.Log($"{string.Format(prefix, infoPrefix)} {string.Format(message, arguments)}");
            Unity.LogException(exception);
        }

        public void Warning(string message, params object[] arguments)
        {
            Unity.LogWarning($"{string.Format(prefix, warnPrefix)} {string.Format(message, arguments)}");
        }

        public void Warning(string message, Exception exception, params object[] arguments)
        {
            Unity.LogWarning($"{string.Format(prefix, warnPrefix)} {string.Format(message, arguments)}");
            Unity.LogException(exception);
        }

        public void Trace(string message, params object[] arguments)
        {
            Info(message, arguments);
        }

        public void Trace(string message, Exception exception, params object[] arguments)
        {
            Info(message, exception, arguments);
        }

        public void Debug(string message, params object[] arguments)
        {
            Info(message, arguments);
        }

        public void Debug(string message, Exception exception, params object[] arguments)
        {
            Info(message, exception, arguments);
        }
    }
}
