using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteAdminInstanceManager : adminInstanceInterface
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;

        // Static field to track last update time
        private static DateTime _lastUpdate = DateTime.MinValue;
        public bool addAdminAccount(string username, string password, AdminRoles role) => CmdaddAdminAccount.ProcessCommand(username, password, (int)role);

        void adminInstanceInterface.AddLogEntry(int userId, string action)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        bool adminInstanceInterface.Authenticate(string username, string password)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return false;
        }

        void adminInstanceInterface.CleanupLogs()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        string adminInstanceInterface.EncryptPassword(string password)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return string.Empty;
        }

        void adminInstanceInterface.LoadAdmins()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void adminInstanceInterface.LoadLogs()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void adminInstanceInterface.LoadSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public bool removeAdminAccount(int userId) => CmddeleteAdminAccount.ProcessCommand(userId);


        void adminInstanceInterface.SaveAdmins()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void adminInstanceInterface.SaveLogs()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public bool updateAdminAccount(int userId, string? username = null, string? password = null, AdminRoles? role = null) => CmdeditAdminAccount.ProcessCommand(userId, username!, password!, (int)role!);

        bool adminInstanceInterface.ValidatePassword(string password, string encryptedPassword)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return false;
        }

        public void UpdateAdminLogDialog()
        {
            // TO DO: Remvoe this method if not needed
        }
    }
}
