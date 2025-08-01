using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class adminInstanceManager
    {
        public static adminInstanceInterface Implementation { get; set; }
        // Proxy methods for convenience (optional, but can be helpful for consumers)
        public static void LoadSettings() => Implementation.LoadSettings();
        public static void LoadAdmins() => Implementation.LoadAdmins();
        public static void SaveAdmins() => Implementation.SaveAdmins();
        public static void LoadLogs() => Implementation.LoadLogs();
        public static void SaveLogs() => Implementation.SaveLogs();
        public static void CleanupLogs() => Implementation.CleanupLogs();
        public static void AddLogEntry(int userId, string action) => Implementation.AddLogEntry(userId, action);
        public static string EncryptPassword(string password) => Implementation.EncryptPassword(password);
        public static bool ValidatePassword(string password, string encryptedPassword) => Implementation.ValidatePassword(password, encryptedPassword);
        public static bool addAdminAccount(string username, string password, AdminRoles role) => Implementation.addAdminAccount(username, password, role);
        public static bool removeAdminAccount(int userId) => Implementation.removeAdminAccount(userId);
        public static bool updateAdminAccount(int userId, string username = null, string password = null, AdminRoles? role = null) =>
            Implementation.updateAdminAccount(userId, username, password, role);
        public static bool Authenticate(string username, string password) => Implementation.Authenticate(username, password);
        public static void UpdateAdminLogDialog() => Implementation.UpdateAdminLogDialog();
    }
}
