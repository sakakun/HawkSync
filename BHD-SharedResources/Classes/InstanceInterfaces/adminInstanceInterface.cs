using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.InstanceInterfaces
{
    public interface adminInstanceInterface
    {
        void LoadSettings();
        void LoadAdmins();
        void SaveAdmins();
        void LoadLogs();
        void SaveLogs();
        void CleanupLogs();
        void AddLogEntry(int userId, string action);
        string EncryptPassword(string password);
        bool ValidatePassword(string password, string encryptedPassword);
        bool addAdminAccount(string username, string password, AdminRoles role);
        bool removeAdminAccount(int userId);
        bool updateAdminAccount(int userId, string username = null, string password = null, AdminRoles? role = null);
        bool Authenticate(string username, string password);
        void UpdateAdminLogDialog();
    }
}
