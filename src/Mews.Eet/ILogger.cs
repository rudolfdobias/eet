using System;

namespace Mews.Eet
{
    public class EetLogger
    {
        public EetLogger(Action<string> onError, Action<string> onInfo = null, Action<string> onDebug = null)
        {
            OnError = onError;
            OnInfo = onInfo;
            OnDebug = onDebug;
        }

        public EetLogger(Action<string> eagerLogger)
        {
            OnError = eagerLogger;
            OnInfo = eagerLogger;
            OnDebug = eagerLogger;
        }

        private Action<string> OnError { get; }

        private Action<string> OnInfo { get; }

        private Action<string> OnDebug { get; }

        public void Error(string message)
        {
            Log(OnError, message);
        }

        public void Info(string message)
        {
            Log(OnInfo, message);
        }

        public void Debug(string message)
        {
            Log(OnDebug, message);
        }

        protected void Log(Action<string> loggerAction, string message)
        {
            loggerAction?.Invoke(message);
        }
    }
}
