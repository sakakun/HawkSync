using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using System.Security.Cryptography;
using System.Text.Json;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public class serverAdminInstanceManager : adminInstanceInterface
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance = CommonCore.theInstance!;
        private static adminInstance instanceAdmin = CommonCore.instanceAdmin!;

        // Thread safety locks
        private static readonly object adminLock = new();
        private static readonly object logLock = new();
        // Static field to track last update time
        private static DateTime _lastUpdate = DateTime.MinValue;

        public void LoadSettings()
        {
            adminInstanceManager.LoadAdmins();
            adminInstanceManager.LoadLogs();
        }

        public void LoadAdmins()
        {
            string adminDBPath = Path.Combine(CommonCore.AppDataPath, "adminDB.json");
            lock (adminLock)
            {
                if (File.Exists(adminDBPath))
                {
                    try
                    {
                        string json = File.ReadAllText(adminDBPath);
                        var admins = JsonSerializer.Deserialize<List<AdminAccount>>(json) ?? new List<AdminAccount>();
                        // Ensure console admin exists
                        if (!admins.Any(a => a.UserId == 0))
                        {
                            admins.Insert(0, new AdminAccount
                            {
                                UserId = 0,
                                Username = "Console",
                                Password = string.Empty,
                                Role = AdminRoles.Admin
                            });
                        }
                        instanceAdmin.Admins = admins;
                    }
                    catch (Exception ex)
                    {
                        LogError($"Error loading admins: {ex}");
                        BackupFile(adminDBPath);
                        instanceAdmin.Admins = new List<AdminAccount>
                        {
                            new AdminAccount
                            {
                                UserId = 0,
                                Username = "Console",
                                Password = string.Empty,
                                Role = AdminRoles.Admin
                            }
                        };
                        SaveAdmins();
                    }
                }
                else
                {
                    instanceAdmin.Admins = new List<AdminAccount>
                    {
                        new AdminAccount
                        {
                            UserId = 0,
                            Username = "Console",
                            Password = string.Empty,
                            Role = AdminRoles.Admin
                        }
                    };
                    SaveAdmins();
                }
            }
        }

        public void SaveAdmins()
        {
            string adminDBPath = Path.Combine(CommonCore.AppDataPath, "adminDB.json");
            lock (adminLock)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(instanceAdmin.Admins, options);
                    File.WriteAllText(adminDBPath, json);
                }
                catch (Exception ex)
                {
                    LogError($"Error saving admins: {ex}");
                }
            }
        }

        public void LoadLogs()
        {
            string adminLogPath = Path.Combine(CommonCore.AppDataPath, "adminLogs.json");
            lock (logLock)
            {
                if (File.Exists(adminLogPath))
                {
                    try
                    {
                        string json = File.ReadAllText(adminLogPath);
                        var logs = JsonSerializer.Deserialize<List<AdminLog>>(json);
                        // Decode Action from Base64 after loading
                        instanceAdmin.Logs = logs?.Select(log =>
                        {
                            try
                            {
                                log.Action = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(log.Action));
                            }
                            catch
                            {
                                // If decoding fails, keep the original string
                            }
                            return log;
                        }).ToList() ?? new List<AdminLog>();
                    }
                    catch (Exception ex)
                    {
                        LogError($"Error loading logs: {ex}");
                        BackupFile(adminLogPath);
                        instanceAdmin.Logs = new List<AdminLog>();
                        SaveLogs();
                    }
                }
                else
                {
                    instanceAdmin.Logs = new List<AdminLog>();
                    SaveLogs();
                }
            }
        }

        public void SaveLogs()
        {
            string adminLogPath = Path.Combine(CommonCore.AppDataPath, "adminLogs.json");
            lock (logLock)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    // Encode Action as Base64 before saving
                    var encodedLogs = instanceAdmin.Logs.Select(log => new AdminLog
                    {
                        LogId = log.LogId,
                        Timestamp = log.Timestamp,
                        UserId = log.UserId,
                        Action = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(log.Action))
                    }).ToList();

                    string json = JsonSerializer.Serialize(encodedLogs, options);
                    File.WriteAllText(adminLogPath, json);
                }
                catch (Exception ex)
                {
                    LogError($"Error saving logs: {ex}");
                }
            }
        }

        public void CleanupLogs()
        {
            lock (logLock)
            {
                // Clean up old logs, keeping only the last 1000 entries
                if (instanceAdmin.Logs.Count > 2000)
                {
                    instanceAdmin.Logs = instanceAdmin.Logs.OrderByDescending(log => log.Timestamp).Take(1000).ToList();
                }
            }
        }

        public void AddLogEntry(int userId, string action)
        {
            lock (logLock)
            {
                instanceAdmin.Logs.Add(new AdminLog
                {
                    LogId = instanceAdmin.Logs.Count > 0 ? instanceAdmin.Logs.Max(log => log.LogId) + 1 : 1,
                    UserId = userId,
                    Action = action,
                    Timestamp = DateTime.Now
                });
                SaveLogs();
            }
        }

        public string EncryptPassword(string password)
        {
            // Generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            // Hash the password with the salt using PBKDF2
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Combine salt and hash for storage (Base64 encoding)
            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hash);
            return $"{saltBase64}:{hashBase64}";
        }

        public bool ValidatePassword(string password, string encryptedPassword)
        {
            // Split the stored value into salt and hash
            var parts = encryptedPassword.Split(':');
            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            // Hash the input password with the same salt
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Compare hashes in constant time
            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }

        public bool addAdminAccount(string username, string password, AdminRoles role)
        {
            lock (adminLock)
            {
                // Check if the username already exists
                if (instanceAdmin.Admins.Any(a => a.Username == username))
                {
                    return false; // Username already exists
                }
                // Create a new admin account
                var newAdmin = new AdminAccount
                {
                    UserId = instanceAdmin.Admins.Count > 0 ? instanceAdmin.Admins.Max(a => a.UserId) + 1 : 1,
                    Username = username,
                    Password = EncryptPassword(password),
                    Role = role
                };
                instanceAdmin.Admins.Add(newAdmin);
                SaveAdmins();
                return true;
            }
        }

        public bool removeAdminAccount(int userId)
        {
            if (userId == 0) return false; // Prevent removal of console admin
            lock (adminLock)
            {
                var admin = instanceAdmin.Admins.FirstOrDefault(a => a.UserId == userId);
                if (admin == null) return false;
                instanceAdmin.Admins.Remove(admin);
                SaveAdmins();
                return true;
            }
        }

        public bool updateAdminAccount(int userId, string? username = null, string? password = null, AdminRoles? role = null)
        {
            if (userId == 0) return false; // Prevent modification of console admin
            lock (adminLock)
            {
                var admin = instanceAdmin.Admins.FirstOrDefault(a => a.UserId == userId);
                if (admin == null) return false;
                if (username != null && username != string.Empty && username != admin.Username) admin.Username = username;
                if (password != null && password != string.Empty && !ValidatePassword(password, admin.Password!)) admin.Password = EncryptPassword(password);
                if (role.HasValue) admin.Role = role.Value;
                SaveAdmins();
                return true;
            }
        }

        public bool Authenticate(string username, string password)
        {
            lock (adminLock)
            {
                var admin = instanceAdmin.Admins.FirstOrDefault(a => a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (admin == null)
                    return false;

                return ValidatePassword(password, admin.Password);
            }
        }

        // Error logging helper
        private void LogError(string message)
        {
            try
            {
                string errorLogPath = Path.Combine(CommonCore.AppDataPath, "error.log");
                File.AppendAllText(errorLogPath, $"[{DateTime.Now}] {message}{Environment.NewLine}");
            }
            catch
            {
                // If logging fails, do nothing (avoid recursive errors)
            }
        }

        // Backup helper
        private void BackupFile(string filePath)
        {
            try
            {
                string backupPath = filePath + ".bak";
                File.Copy(filePath, backupPath, true);
            }
            catch
            {
                // If backup fails, do nothing
            }
        }

        public void UpdateAdminLogDialog()
        {
            // Only update if 15 seconds have passed
            if ((DateTime.UtcNow - _lastUpdate).TotalSeconds < 15)
                return;

            _lastUpdate = DateTime.UtcNow;

            // Clear the existing rows
            thisServer.dg_AdminUsers.Rows.Clear();
            // Populate the DataGridView with the admin accounts
            foreach (var admin in instanceAdmin.Admins)
            {
                thisServer.dg_AdminUsers.Rows.Add(admin.UserId, admin.Username, admin.Role.ToString());
            }

            thisServer.dg_adminLog.Rows.Clear();

            foreach (AdminLog log in instanceAdmin.Logs
            .OrderByDescending(l => l.Timestamp)
            .Take(50))
            {
                var admin = instanceAdmin.Admins.FirstOrDefault(a => a.UserId == log.UserId);
                string username = admin != null ? admin.Username : $"UserId:{log.UserId}";

                thisServer.dg_adminLog.Rows.Add(
                    log.Timestamp,
                    username,
                    log.Action
                );
            }
        }

    }
}
