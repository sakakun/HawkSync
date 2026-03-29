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

			// If program ran with /debug argument, log to file
            if (Environment.GetCommandLineArgs().Length > 1 && Environment.GetCommandLineArgs()[1].Equals("/debug", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // If logging fails, write to debug output
                    Debug.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
                }
			}

			return;
        }
    }
}
