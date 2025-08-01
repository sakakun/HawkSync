using System;
using System.Diagnostics;
using System.IO;

namespace BHD_SharedResources.Classes.SupportClasses
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
            /*
            try
            {
                // Append the log message to the file
                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Handle any file I/O errors gracefully
                Debug.WriteLine($"[ERROR][AppDebug] {DateTime.Now}: Failed to write to log file. {ex.Message}");
            }
            */
        }
    }
}
