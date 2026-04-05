using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HawkSyncShared.SupportClasses
{
    public static class AppDebug
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppDebug.log");
        private static readonly string EventSource = AppDomain.CurrentDomain.FriendlyName;

        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// Central logging method.
        /// - When a debugger is attached, messages are written to Debug.WriteLine.
        /// - When the process was started with the "/debug" argument, messages are appended to a log file.
        /// - Errors: when a debugger is NOT attached, attempts to write to the Windows Event Log; falls back to file/debug output on failure.
        /// - Caller info (class, method, line) is captured automatically.
        /// </summary>
        public static void Log(
            string message,
            LogLevel level = LogLevel.Debug,
            Exception? exception = null,
            [CallerMemberName] string callerMember = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            string className = Path.GetFileNameWithoutExtension(callerFilePath);
            string time = DateTime.Now.ToString("o"); // ISO 8601
            string exceptionPart = exception is null ? string.Empty : $"{Environment.NewLine}Exception: {exception}";
            string formatted = $"[{level}][{className}.{callerMember}:line {callerLineNumber}] {time} - {message}{exceptionPart}";

            bool isDebuggerAttached = Debugger.IsAttached;
            bool fileLoggingRequested;

            try
            {
                string[] args = Environment.GetCommandLineArgs();
                fileLoggingRequested = Array.Exists(args, a => a.Equals("/debug", StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                fileLoggingRequested = false;
            }

            // If user asked for /debug, always attempt file logging (best-effort).
            if (fileLoggingRequested)
            {
                try
                {
                    File.AppendAllText(LogFilePath, formatted + Environment.NewLine);
                }
                catch (Exception fileEx)
                {
                    // If file logging fails, write that failure to Debug output so it's visible during development.
                    Debug.WriteLine($"[AppDebug][ERROR] Failed to write to log file: {fileEx.Message}");
                }
            }

            // Handle error-level specially:
            if (level == LogLevel.Error)
            {
                if (isDebuggerAttached)
                {
                    // In debug sessions, show errors in debug output (developer gets immediate feedback).
                    Debug.WriteLine(formatted);
                    return;
                }

                // Production: try Windows Event Log (requires appropriate privileges)
                try
                {
                    if (!EventLog.SourceExists(EventSource))
                    {
                        // Creating a source requires admin rights on Windows; if it fails we'll fallback below.
                        EventLog.CreateEventSource(new EventSourceCreationData(EventSource, "Application"));
                    }

                    EventLog.WriteEntry(EventSource, formatted, EventLogEntryType.Error);
                    return;
                }
                catch (Exception eventEx)
                {
                    // If Event Log fails (insufficient privileges, etc.), fallback to file and debug output.
                    try
                    {
                        File.AppendAllText(LogFilePath, $"[FALLBACK][EventLogFailure] {formatted} -- Exception: {eventEx.Message}{Environment.NewLine}");
                    }
                    catch
                    {
                        // ignore, we will at least write to Debug.
                    }

                    Debug.WriteLine(formatted);
                    Debug.WriteLine($"[AppDebug][ERROR] Failed to write to Event Log: {eventEx.Message}");
                    return;
                }
            }

            // Non-error messages: only write to Debug output when attached (file already handled above if /debug).
            if (isDebuggerAttached)
            {
                Debug.WriteLine(formatted);
            }
        }

        // Convenience wrappers
        public static void DebugMsg(string message, Exception? exception = null, [CallerMemberName] string callerMember = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            => Log(message, LogLevel.Debug, exception, callerMember, callerFilePath, callerLineNumber);

        public static void Info(string message, Exception? exception = null, [CallerMemberName] string callerMember = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            => Log(message, LogLevel.Info, exception, callerMember, callerFilePath, callerLineNumber);

        public static void Warn(string message, Exception? exception = null, [CallerMemberName] string callerMember = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            => Log(message, LogLevel.Warning, exception, callerMember, callerFilePath, callerLineNumber);

        public static void Error(string message, Exception? exception = null, [CallerMemberName] string callerMember = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            => Log(message, LogLevel.Error, exception, callerMember, callerFilePath, callerLineNumber);
    }
}