using System;
using System.Diagnostics;
using System.IO;

namespace HawkSyncShared.SupportClasses
{
    public static class AppDebug
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppDebug.log");

        public static void Log(string classString, string debugMessage)
        {
            string logMessage = $"[DEBUG][{classString}] {DateTime.Now}: {debugMessage}";

            if (Debugger.IsAttached)
            {
                Debug.WriteLine(logMessage);
                return;
            }

            return;
        }
    }
}
