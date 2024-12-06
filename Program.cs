using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using ServerManager.Classes.Enviroment;
using ServerManager.Panels;

namespace ServerManager;

public class Program
{
    public static ServerEnvironment ServerEnvironment;
    
    [STAThread]
    static void Main()
    {
        if (!IsRunningAsAdmin())
        {
            RestartAsAdmin();
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Panels.ServerManager());
    }

    private static bool IsRunningAsAdmin()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static void RestartAsAdmin()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Assembly.GetExecutingAssembly().Location,
            UseShellExecute = true,
            Verb = "runas" // Request elevation
        };

        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            DebugLog("Failed to restart as admin: " + ex.Message);
        }
    }

    public static void DebugLog(string message)
    {
        string logFilePath = "./ServerManager.log"; // Specify your log file path

        string source = Assembly.GetExecutingAssembly().GetName().Name;
        bool isDebugMode = Environment.GetCommandLineArgs().Contains("/debug");

        if (isDebugMode)
        {
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            Console.WriteLine(DateTime.Now + ": " + message);
        }
        else if (Debugger.IsAttached)
        {
            Console.WriteLine(message);
        }
        else
        {
            // Ensure the source exists before writing to the event log
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, "Application");
            }

            EventLog.WriteEntry(source, DateTime.Now + ": " + message, EventLogEntryType.Information);
        }
    }
}