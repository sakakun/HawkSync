using BCrypt.Net;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.DTOs.tabAdmin;

namespace BHD_ServerManager.Classes.InstanceManagers;

public static class adminInstanceManager
{
    private static adminInstance adminInstance => CommonCore.instanceAdmin!;
    private const int DefaultAdminUserId = 1;
    private const int MaxAuditLogEntries = 1000;
    private static readonly string[] ValidPermissions = new[]
    {
        "profile", "gameplay", "maps", "players",
        "chat", "bans", "stats", "users"
    };
    private const int BcryptWorkFactor = 11;

    // --- CACHE MANAGEMENT ---

    public static void LoadUsersCache()
    {
        try
        {
            var userRecords = DatabaseManager.GetAllUsers();
            var users = new List<UserDTO>();

            foreach (var record in userRecords)
            {
                var permissions = DatabaseManager.GetUserPermissions(record.UserID);
                users.Add(new UserDTO
                {
                    UserID = record.UserID,
                    Username = record.Username,
                    IsActive = record.IsActive,
                    Created = record.Created,
                    LastLogin = record.LastLogin,
                    Notes = record.Notes,
                    Permissions = permissions
                });
            }

            adminInstance.Users = users;
            AppDebug.Log("adminInstanceManager", $"Loaded {users.Count} users into cache");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error loading users cache: {ex.Message}");
            adminInstance.Users = new List<UserDTO>();
        }
    }

    private static void RefreshUserInCache(int userID)
    {
        try
        {
            var record = DatabaseManager.GetUserByID(userID);
            if (record == null)
            {
                adminInstance.Users.RemoveAll(u => u.UserID == userID);
                return;
            }

            var permissions = DatabaseManager.GetUserPermissions(record.UserID);

            var userDTO = new UserDTO
            {
                UserID = record.UserID,
                Username = record.Username,
                IsActive = record.IsActive,
                Created = record.Created,
                LastLogin = record.LastLogin,
                Notes = record.Notes,
                Permissions = permissions
            };

            adminInstance.Users.RemoveAll(u => u.UserID == userID);
            adminInstance.Users.Add(userDTO);

            AppDebug.Log("adminInstanceManager", $"Refreshed user {userID} in cache");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error refreshing user in cache: {ex.Message}");
        }
    }

    // --- USER CRUD OPERATIONS ---

    public static List<UserDTO> GetAllUsers() => adminInstance.Users.ToList();

    public static UserDTO? GetUserByID(int userID) =>
        adminInstance.Users.FirstOrDefault(u => u.UserID == userID);

    public static UserDTO? GetUserByUsername(string username) =>
        adminInstance.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

    public static OperationResult CreateUser(CreateUserRequestDTO request)
    {
        try
        {
            var (isValid, errors) = ValidateCreateUserRequest(request);
            if (!isValid)
                return new OperationResult(false, string.Join("\n", errors));

            if (DatabaseManager.UsernameExists(request.Username))
                return new OperationResult(false, "Username already exists.");

            var invalidPermissions = request.Permissions
                .Where(p => !ValidPermissions.Contains(p.ToLower()))
                .ToList();

            if (invalidPermissions.Any())
                return new OperationResult(false, $"Invalid permissions: {string.Join(", ", invalidPermissions)}");

            string salt = BCrypt.Net.BCrypt.GenerateSalt(BcryptWorkFactor);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            int userID = DatabaseManager.AddUser(
                request.Username,
                passwordHash,
                salt,
                request.IsActive,
                request.Notes ?? string.Empty
            );

            if (userID <= 0)
                return new OperationResult(false, "Failed to create user.");

            foreach (var permission in request.Permissions)
                DatabaseManager.AddUserPermission(userID, permission.ToLower());

            RefreshUserInCache(userID);
            LogAudit("system", "USER_CREATED", $"User '{request.Username}' created with ID {userID}");

            AppDebug.Log("adminInstanceManager", $"Created user: {request.Username} (ID: {userID}) with {request.Permissions.Count} permissions");
            return new OperationResult(true, $"User '{request.Username}' created successfully.", userID);
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error creating user: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }

    public static OperationResult UpdateUser(UpdateUserRequestDTO request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                return new OperationResult(false, "Username cannot be empty.");

            if (request.Username.Length < 3 || request.Username.Length > 50)
                return new OperationResult(false, "Username must be between 3 and 50 characters.");

            var existingCachedUser = GetUserByID(request.UserID);
            if (existingCachedUser == null)
            {
                var existingUser = DatabaseManager.GetUserByID(request.UserID);
                if (existingUser == null)
                    return new OperationResult(false, "User not found.");
            }

            if (DatabaseManager.UsernameExistsForOtherUser(request.Username, request.UserID))
                return new OperationResult(false, "Username already exists.");

            var invalidPermissions = request.Permissions
                .Where(p => !ValidPermissions.Contains(p.ToLower()))
                .ToList();

            if (invalidPermissions.Any())
                return new OperationResult(false, $"Invalid permissions: {string.Join(", ", invalidPermissions)}");

            bool updateSuccess = DatabaseManager.UpdateUser(
                request.UserID,
                request.Username,
                request.IsActive,
                request.Notes ?? string.Empty
            );

            if (!updateSuccess)
                return new OperationResult(false, "Failed to update user.");

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                var (pwdValid, pwdErrors) = ValidatePassword(request.NewPassword);
                if (!pwdValid)
                    return new OperationResult(false, string.Join("\n", pwdErrors));

                string salt = BCrypt.Net.BCrypt.GenerateSalt(BcryptWorkFactor);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);

                DatabaseManager.UpdateUserPassword(request.UserID, passwordHash, salt);
            }

            DatabaseManager.DeleteAllUserPermissions(request.UserID);
            foreach (var permission in request.Permissions)
                DatabaseManager.AddUserPermission(request.UserID, permission.ToLower());

            RefreshUserInCache(request.UserID);
            LogAudit("system", "USER_UPDATED", $"User ID {request.UserID} ({request.Username}) updated");

            AppDebug.Log("adminInstanceManager", $"Updated user ID: {request.UserID} ({request.Username}) with {request.Permissions.Count} permissions");
            return new OperationResult(true, $"User '{request.Username}' updated successfully.", request.UserID);
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error updating user: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }

    public static OperationResult DeleteUser(int userID)
    {
        try
        {
            if (userID == DefaultAdminUserId)
                return new OperationResult(false, "Cannot delete the default admin user.");

            var user = GetUserByID(userID);
            if (user == null)
            {
                var dbUser = DatabaseManager.GetUserByID(userID);
                if (dbUser == null)
                    return new OperationResult(false, "User not found.");
                user = new UserDTO { UserID = dbUser.UserID, Username = dbUser.Username };
            }

            bool deleteSuccess = DatabaseManager.DeleteUser(userID);

            if (!deleteSuccess)
                return new OperationResult(false, "Failed to delete user.");

            adminInstance.Users.RemoveAll(u => u.UserID == userID);
            LogAudit("system", "USER_DELETED", $"User ID {userID} ({user.Username}) deleted");

            AppDebug.Log("adminInstanceManager", $"Deleted user ID: {userID} ({user.Username})");
            return new OperationResult(true, $"User '{user.Username}' deleted successfully.");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error deleting user: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }

    // --- AUTHENTICATION ---

    public static (bool success, UserDTO? user, string message) AuthenticateUser(string username, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, null, "Username and password are required.");

            var userRecord = DatabaseManager.GetUserByUsername(username);

            if (userRecord == null)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: User not found - {username}");
                LogAudit(username, "LOGIN_FAILED", "User not found");
                return (false, null, "Invalid username or password.");
            }

            if (!userRecord.IsActive)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: User disabled - {username}");
                LogAudit(username, "LOGIN_FAILED", "Account disabled");
                return (false, null, "This account has been disabled.");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, userRecord.PasswordHash);

            if (!passwordValid)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: Invalid password - {username}");
                LogAudit(username, "LOGIN_FAILED", "Invalid password");
                return (false, null, "Invalid username or password.");
            }

            DatabaseManager.UpdateUserLastLogin(userRecord.UserID);

            var permissions = DatabaseManager.GetUserPermissions(userRecord.UserID);

            var userDTO = new UserDTO
            {
                UserID = userRecord.UserID,
                Username = userRecord.Username,
                IsActive = userRecord.IsActive,
                Created = userRecord.Created,
                LastLogin = DateTime.Now,
                Notes = userRecord.Notes,
                Permissions = permissions
            };

            RefreshUserInCache(userRecord.UserID);
            LogAudit(username, "LOGIN_SUCCESS", "User authenticated successfully");

            AppDebug.Log("adminInstanceManager", $"User authenticated successfully: {username} (ID: {userRecord.UserID})");
            return (true, userDTO, "Authentication successful.");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Authentication error: {ex.Message}");
            return (false, null, $"Authentication error: {ex.Message}");
        }
    }

    // --- AUDIT LOGGING ---

    private static void LogAudit(string username, string action, string details)
    {
        try
        {
            var auditEntry = new AdminAuditLog
            {
                Timestamp = DateTime.Now,
                Username = username,
                Action = action,
                Details = details
            };

            adminInstance.AuditLog.Add(auditEntry);

            if (adminInstance.AuditLog.Count > MaxAuditLogEntries)
                adminInstance.AuditLog.RemoveAt(0);

            AppDebug.Log("adminInstanceManager", $"AUDIT: [{username}] {action} - {details}");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error logging audit: {ex.Message}");
        }
    }

    public static List<AdminAuditLog> GetRecentAuditLog(int count = 50) =>
        adminInstance.AuditLog.OrderByDescending(x => x.Timestamp).Take(count).ToList();

    // --- SESSION MANAGEMENT ---

    public static void TrackSession(string username)
    {
        adminInstance.ActiveSessions[username] = DateTime.Now;
        AppDebug.Log("adminInstanceManager", $"Session tracked for user: {username}");
    }

    public static void RemoveSession(string username)
    {
        if (adminInstance.ActiveSessions.Remove(username))
        {
            LogAudit("system", "SESSION_REMOVED", $"Session removed for user: {username}");
            AppDebug.Log("adminInstanceManager", $"Session removed for user: {username}");
        }
    }

    public static List<(string Username, DateTime LoginTime, TimeSpan Duration)> GetActiveSessions() =>
        adminInstance.ActiveSessions
            .Select(kvp => (kvp.Key, kvp.Value, DateTime.Now - kvp.Value))
            .OrderByDescending(x => x.Item2)
            .ToList();

    // --- VALIDATION ---

    public static (bool isValid, List<string> errors) ValidateCreateUserRequest(CreateUserRequestDTO request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Username))
            errors.Add("Username cannot be empty.");
        else if (request.Username.Length < 3)
            errors.Add("Username must be at least 3 characters.");
        else if (request.Username.Length > 50)
            errors.Add("Username cannot exceed 50 characters.");
        else if (!System.Text.RegularExpressions.Regex.IsMatch(request.Username, @"^[a-zA-Z0-9_-]+$"))
            errors.Add("Username can only contain letters, numbers, underscores, and hyphens.");

        var (pwdValid, pwdErrors) = ValidatePassword(request.Password);
        if (!pwdValid)
            errors.AddRange(pwdErrors);

        return (errors.Count == 0, errors);
    }

    public static (bool isValid, List<string> errors) ValidatePassword(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
            errors.Add("Password cannot be empty.");
        else if (password.Length < 6)
            errors.Add("Password must be at least 6 characters.");
        else if (password.Length > 100)
            errors.Add("Password cannot exceed 100 characters.");

        return (errors.Count == 0, errors);
    }

    // --- HELPER METHODS ---

    public static List<string> GetDefaultPermissionsForRole(string role)
    {
        return role.ToLower() switch
        {
            "admin" => ValidPermissions.ToList(),
            "moderator" => new List<string> { "players", "chat", "bans" },
            "viewer" => new List<string> { "stats" },
            _ => new List<string>()
        };
    }

    public static bool UserHasPermission(int userID, string permission)
    {
        var user = GetUserByID(userID);
        return user?.Permissions.Contains(permission.ToLower()) ?? false;
    }

    public static bool UserHasPermission(UserDTO user, string permission) =>
        user.Permissions.Contains(permission.ToLower());

    public static List<string> GetValidPermissions() => ValidPermissions.ToList();

    public static OperationResult InitializeDefaultAdmin()
    {
        try
        {
            var users = DatabaseManager.GetAllUsers();

            if (users.Count > 0)
            {
                AppDebug.Log("adminInstanceManager", "Users already exist, skipping default admin creation");
                LoadUsersCache();
                return new OperationResult(true, "Users already exist.");
            }

            var request = new CreateUserRequestDTO
            {
                Username = "admin",
                Password = "admin",
                Permissions = GetDefaultPermissionsForRole("admin"),
                IsActive = true,
                Notes = "Default administrator account - CHANGE PASSWORD IMMEDIATELY"
            };

            var result = CreateUser(request);

            if (result.Success)
            {
                AppDebug.Log("adminInstanceManager", "Created default admin user (username: admin, password: admin)");
                LoadUsersCache();
            }

            return result;
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error initializing default admin: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }
}