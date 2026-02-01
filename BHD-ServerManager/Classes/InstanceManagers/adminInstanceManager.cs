using BCrypt.Net;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.DTOs;

namespace BHD_ServerManager.Classes.InstanceManagers;

/// <summary>
/// Manager for user account operations and authentication
/// </summary>
public static class adminInstanceManager
{
    private static adminInstance adminInstance => CommonCore.instanceAdmin!;
    // Valid permissions that can be assigned to users
    private static readonly string[] ValidPermissions = new[]
    {
        "profile", "gameplay", "maps", "players",
        "chat", "bans", "stats", "users"
    };

    // BCrypt work factor (higher = more secure but slower)
    private const int BcryptWorkFactor = 11;

    // ================================================================================
    // DATA TRANSFER OBJECTS (DTOs)
    // ================================================================================

    // ================================================================================
    // CACHE MANAGEMENT
    // ================================================================================

    /// <summary>
    /// Load all users from database into adminInstance.Users cache
    /// </summary>
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

    /// <summary>
    /// Refresh a specific user in the cache
    /// </summary>
    private static void RefreshUserInCache(int userID)
    {
        try
        {
            var record = DatabaseManager.GetUserByID(userID);
            if (record == null)
            {
                // User was deleted, remove from cache
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

            // Remove old entry and add updated one
            adminInstance.Users.RemoveAll(u => u.UserID == userID);
            adminInstance.Users.Add(userDTO);

            AppDebug.Log("adminInstanceManager", $"Refreshed user {userID} in cache");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error refreshing user in cache: {ex.Message}");
        }
    }

    // ================================================================================
    // USER CRUD OPERATIONS
    // ================================================================================

    /// <summary>
    /// Get all users from cache (use this for UI/API)
    /// </summary>
    public static List<UserDTO> GetAllUsers()
    {
        return adminInstance.Users.ToList();
    }

    /// <summary>
    /// Get a single user by UserID from cache
    /// </summary>
    public static UserDTO? GetUserByID(int userID)
    {
        return adminInstance.Users.FirstOrDefault(u => u.UserID == userID);
    }

    /// <summary>
    /// Get a single user by Username from cache
    /// </summary>
    public static UserDTO? GetUserByUsername(string username)
    {
        return adminInstance.Users
            .FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Create a new user account
    /// </summary>
    public static OperationResult CreateUser(CreateUserRequest request)
    {
        try
        {
            // Validate input
            var (isValid, errors) = ValidateCreateUserRequest(request);
            if (!isValid)
                return new OperationResult(false, string.Join("\n", errors));

            // Check if username already exists
            if (DatabaseManager.UsernameExists(request.Username))
                return new OperationResult(false, "Username already exists.");

            // Validate permissions
            var invalidPermissions = request.Permissions
                .Where(p => !ValidPermissions.Contains(p.ToLower()))
                .ToList();

            if (invalidPermissions.Any())
                return new OperationResult(false, 
                    $"Invalid permissions: {string.Join(", ", invalidPermissions)}");

            // Hash password
            string salt = BCrypt.Net.BCrypt.GenerateSalt(BcryptWorkFactor);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            // Add user to database
            int userID = DatabaseManager.AddUser(
                request.Username,
                passwordHash,
                salt,
                request.IsActive,
                request.Notes ?? string.Empty
            );

            if (userID <= 0)
                return new OperationResult(false, "Failed to create user.");

            // Add permissions
            foreach (var permission in request.Permissions)
            {
                DatabaseManager.AddUserPermission(userID, permission.ToLower());
            }

            // Refresh cache with new user
            RefreshUserInCache(userID);

            // Log audit
            LogAudit("system", "USER_CREATED", 
                $"User '{request.Username}' created with ID {userID}");

            AppDebug.Log("adminInstanceManager", 
                $"Created user: {request.Username} (ID: {userID}) with {request.Permissions.Count} permissions");

            return new OperationResult(true, 
                $"User '{request.Username}' created successfully.", userID);
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error creating user: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }

    /// <summary>
    /// Update an existing user account
    /// </summary>
    public static OperationResult UpdateUser(UpdateUserRequest request)
    {
        try
        {
            // Validate username
            if (string.IsNullOrWhiteSpace(request.Username))
                return new OperationResult(false, "Username cannot be empty.");

            if (request.Username.Length < 3 || request.Username.Length > 50)
                return new OperationResult(false, "Username must be between 3 and 50 characters.");

            // Check if user exists in cache
            var existingCachedUser = GetUserByID(request.UserID);
            if (existingCachedUser == null)
            {
                // Fallback to database check
                var existingUser = DatabaseManager.GetUserByID(request.UserID);
                if (existingUser == null)
                    return new OperationResult(false, "User not found.");
            }

            // Check if username is taken by another user
            if (DatabaseManager.UsernameExistsForOtherUser(request.Username, request.UserID))
                return new OperationResult(false, "Username already exists.");

            // Validate permissions
            var invalidPermissions = request.Permissions
                .Where(p => !ValidPermissions.Contains(p.ToLower()))
                .ToList();

            if (invalidPermissions.Any())
                return new OperationResult(false, 
                    $"Invalid permissions: {string.Join(", ", invalidPermissions)}");

            // Update user details
            bool updateSuccess = DatabaseManager.UpdateUser(
                request.UserID,
                request.Username,
                request.IsActive,
                request.Notes ?? string.Empty
            );

            if (!updateSuccess)
                return new OperationResult(false, "Failed to update user.");

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                var (pwdValid, pwdErrors) = ValidatePassword(request.NewPassword);
                if (!pwdValid)
                    return new OperationResult(false, string.Join("\n", pwdErrors));

                string salt = BCrypt.Net.BCrypt.GenerateSalt(BcryptWorkFactor);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);

                DatabaseManager.UpdateUserPassword(request.UserID, passwordHash, salt);
            }

            // Update permissions
            DatabaseManager.DeleteAllUserPermissions(request.UserID);
            foreach (var permission in request.Permissions)
            {
                DatabaseManager.AddUserPermission(request.UserID, permission.ToLower());
            }

            // Refresh cache with updated user
            RefreshUserInCache(request.UserID);

            // Log audit
            LogAudit("system", "USER_UPDATED", 
                $"User ID {request.UserID} ({request.Username}) updated");

            AppDebug.Log("adminInstanceManager", 
                $"Updated user ID: {request.UserID} ({request.Username}) with {request.Permissions.Count} permissions");

            return new OperationResult(true, 
                $"User '{request.Username}' updated successfully.", request.UserID);
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error updating user: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }

    /// <summary>
    /// Delete a user account
    /// </summary>
    public static OperationResult DeleteUser(int userID)
    {
        try
        {
            // Prevent deleting the default admin (UserID = 1)
            if (userID == 1)
                return new OperationResult(false, 
                    "Cannot delete the default admin user.");

            // Check if user exists in cache
            var user = GetUserByID(userID);
            if (user == null)
            {
                // Fallback to database check
                var dbUser = DatabaseManager.GetUserByID(userID);
                if (dbUser == null)
                    return new OperationResult(false, "User not found.");
                
                user = new UserDTO { UserID = dbUser.UserID, Username = dbUser.Username };
            }

            // Delete user (permissions will cascade delete)
            bool deleteSuccess = DatabaseManager.DeleteUser(userID);

            if (!deleteSuccess)
                return new OperationResult(false, "Failed to delete user.");

            // Remove from cache
            adminInstance.Users.RemoveAll(u => u.UserID == userID);

            // Log audit
            LogAudit("system", "USER_DELETED", 
                $"User ID {userID} ({user.Username}) deleted");

            AppDebug.Log("adminInstanceManager", 
                $"Deleted user ID: {userID} ({user.Username})");

            return new OperationResult(true, 
                $"User '{user.Username}' deleted successfully.");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error deleting user: {ex.Message}");
            return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
        }
    }

    // ================================================================================
    // AUTHENTICATION
    // ================================================================================

    /// <summary>
    /// Authenticate a user with username and password
    /// </summary>
    public static (bool success, UserDTO? user, string message) AuthenticateUser(
        string username, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, null, "Username and password are required.");

            // Get user from database (can't use cache since we need password hash)
            var userRecord = DatabaseManager.GetUserByUsername(username);
            
            if (userRecord == null)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: User not found - {username}");
                LogAudit(username, "LOGIN_FAILED", "User not found");
                return (false, null, "Invalid username or password.");
            }

            // Check if user is active
            if (!userRecord.IsActive)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: User disabled - {username}");
                LogAudit(username, "LOGIN_FAILED", "Account disabled");
                return (false, null, "This account has been disabled.");
            }

            // Verify password using BCrypt
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, userRecord.PasswordHash);
            
            if (!passwordValid)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: Invalid password - {username}");
                LogAudit(username, "LOGIN_FAILED", "Invalid password");
                return (false, null, "Invalid username or password.");
            }

            // Update last login timestamp
            DatabaseManager.UpdateUserLastLogin(userRecord.UserID);

            // Load permissions
            var permissions = DatabaseManager.GetUserPermissions(userRecord.UserID);

            // Create UserDTO
            var userDTO = new UserDTO
            {
                UserID = userRecord.UserID,
                Username = userRecord.Username,
                IsActive = userRecord.IsActive,
                Created = userRecord.Created,
                LastLogin = DateTime.Now, // Just updated
                Notes = userRecord.Notes,
                Permissions = permissions
            };

            // Refresh cache with updated login time
            RefreshUserInCache(userRecord.UserID);

            // Log audit
            LogAudit(username, "LOGIN_SUCCESS", "User authenticated successfully");

            AppDebug.Log("adminInstanceManager", 
                $"User authenticated successfully: {username} (ID: {userRecord.UserID})");

            return (true, userDTO, "Authentication successful.");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Authentication error: {ex.Message}");
            return (false, null, $"Authentication error: {ex.Message}");
        }
    }

    // ================================================================================
    // AUDIT LOGGING
    // ================================================================================

    /// <summary>
    /// Log an audit entry
    /// </summary>
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

            // Keep only last 1000 entries in memory
            if (adminInstance.AuditLog.Count > 1000)
            {
                adminInstance.AuditLog.RemoveAt(0);
            }

            // Optionally persist to database
            // DatabaseManager.AddAuditLog(auditEntry);

            AppDebug.Log("adminInstanceManager", $"AUDIT: [{username}] {action} - {details}");
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error logging audit: {ex.Message}");
        }
    }

    /// <summary>
    /// Get recent audit log entries
    /// </summary>
    public static List<AdminAuditLog> GetRecentAuditLog(int count = 50)
    {
        return adminInstance.AuditLog
            .OrderByDescending(x => x.Timestamp)
            .Take(count)
            .ToList();
    }

    // ================================================================================
    // SESSION MANAGEMENT
    // ================================================================================

    /// <summary>
    /// Track a user session (for remote clients)
    /// </summary>
    public static void TrackSession(string username)
    {
        adminInstance.ActiveSessions[username] = DateTime.Now;
        AppDebug.Log("adminInstanceManager", $"Session tracked for user: {username}");
    }

    /// <summary>
    /// Remove a user session
    /// </summary>
    public static void RemoveSession(string username)
    {
        if (adminInstance.ActiveSessions.Remove(username))
        {
            LogAudit("system", "SESSION_REMOVED", $"Session removed for user: {username}");
            AppDebug.Log("adminInstanceManager", $"Session removed for user: {username}");
        }
    }

    /// <summary>
    /// Get active sessions
    /// </summary>
    public static List<(string Username, DateTime LoginTime, TimeSpan Duration)> GetActiveSessions()
    {
        return adminInstance.ActiveSessions
            .Select(kvp => (
                Username: kvp.Key,
                LoginTime: kvp.Value,
                Duration: DateTime.Now - kvp.Value
            ))
            .OrderByDescending(x => x.LoginTime)
            .ToList();
    }

    // ================================================================================
    // VALIDATION
    // ================================================================================

    /// <summary>
    /// Validate a CreateUserRequest
    /// </summary>
    public static (bool isValid, List<string> errors) ValidateCreateUserRequest(
        CreateUserRequest request)
    {
        var errors = new List<string>();

        // Username validation
        if (string.IsNullOrWhiteSpace(request.Username))
            errors.Add("Username cannot be empty.");
        else if (request.Username.Length < 3)
            errors.Add("Username must be at least 3 characters.");
        else if (request.Username.Length > 50)
            errors.Add("Username cannot exceed 50 characters.");
        else if (!System.Text.RegularExpressions.Regex.IsMatch(
            request.Username, @"^[a-zA-Z0-9_-]+$"))
            errors.Add("Username can only contain letters, numbers, underscores, and hyphens.");

        // Password validation
        var (pwdValid, pwdErrors) = ValidatePassword(request.Password);
        if (!pwdValid)
            errors.AddRange(pwdErrors);

        return (errors.Count == 0, errors);
    }

    /// <summary>
    /// Validate a password
    /// </summary>
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

    // ================================================================================
    // HELPER METHODS
    // ================================================================================

    /// <summary>
    /// Get default permissions for a role (for future role-based access)
    /// </summary>
    public static List<string> GetDefaultPermissionsForRole(string role)
    {
        return role.ToLower() switch
        {
            "admin" => new List<string> 
            { 
                "profile", "gameplay", "maps", "players", 
                "chat", "bans", "stats", "users" 
            },
            "moderator" => new List<string> 
            { 
                "players", "chat", "bans" 
            },
            "viewer" => new List<string> 
            { 
                "stats" 
            },
            _ => new List<string>()
        };
    }

    /// <summary>
    /// Check if a user has a specific permission (by UserID)
    /// </summary>
    public static bool UserHasPermission(int userID, string permission)
    {
        var user = GetUserByID(userID);
        return user?.Permissions.Contains(permission.ToLower()) ?? false;
    }

    /// <summary>
    /// Check if a user has a specific permission (by UserDTO)
    /// </summary>
    public static bool UserHasPermission(UserDTO user, string permission)
    {
        return user.Permissions.Contains(permission.ToLower());
    }

    /// <summary>
    /// Get all valid permissions
    /// </summary>
    public static List<string> GetValidPermissions()
    {
        return ValidPermissions.ToList();
    }

    /// <summary>
    /// Initialize default admin user if no users exist
    /// </summary>
    public static OperationResult InitializeDefaultAdmin()
    {
        try
        {
            var users = DatabaseManager.GetAllUsers();
            
            if (users.Count > 0)
            {
                AppDebug.Log("adminInstanceManager", "Users already exist, skipping default admin creation");
                
                // Load existing users into cache
                LoadUsersCache();
                
                return new OperationResult(true, "Users already exist.");
            }

            // Create default admin
            var request = new CreateUserRequest
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
                AppDebug.Log("adminInstanceManager", 
                    "Created default admin user (username: admin, password: admin)");
                
                // Load cache after creating first user
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