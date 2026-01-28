using BCrypt.Net;
using BHD_ServerManager.Classes.SupportClasses;

namespace BHD_ServerManager.Classes.InstanceManagers;

/// <summary>
/// Manager for user account operations and authentication
/// </summary>
public static class adminInstanceManager
{
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

    /// <summary>
    /// User data transfer object for UI display
    /// </summary>
    public record UserDTO
    {
        public int UserID { get; init; }
        public string Username { get; init; } = string.Empty;
        public List<string> Permissions { get; init; } = new();
        public bool IsActive { get; init; } = true;
        public DateTime Created { get; init; }
        public DateTime? LastLogin { get; init; }
        public string Notes { get; init; } = string.Empty;
    }

    /// <summary>
    /// Request for creating a new user
    /// </summary>
    public record CreateUserRequest(
        string Username,
        string Password,
        List<string> Permissions,
        bool IsActive,
        string Notes
    );

    /// <summary>
    /// Request for updating an existing user
    /// </summary>
    public record UpdateUserRequest(
        int UserID,
        string Username,
        string? NewPassword, // null = don't change password
        List<string> Permissions,
        bool IsActive,
        string Notes
    );

    // ================================================================================
    // USER CRUD OPERATIONS
    // ================================================================================

    /// <summary>
    /// Get all users from database
    /// </summary>
    public static List<UserDTO> GetAllUsers()
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

            AppDebug.Log("adminInstanceManager", $"Retrieved {users.Count} users");
            return users;
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error getting all users: {ex.Message}");
            return new List<UserDTO>();
        }
    }

    /// <summary>
    /// Get a single user by UserID
    /// </summary>
    public static UserDTO? GetUserByID(int userID)
    {
        try
        {
            var record = DatabaseManager.GetUserByID(userID);
            if (record == null)
                return null;

            var permissions = DatabaseManager.GetUserPermissions(record.UserID);

            return new UserDTO
            {
                UserID = record.UserID,
                Username = record.Username,
                IsActive = record.IsActive,
                Created = record.Created,
                LastLogin = record.LastLogin,
                Notes = record.Notes,
                Permissions = permissions
            };
        }
        catch (Exception ex)
        {
            AppDebug.Log("adminInstanceManager", $"Error getting user by ID: {ex.Message}");
            return null;
        }
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

            // Check if user exists
            var existingUser = DatabaseManager.GetUserByID(request.UserID);
            if (existingUser == null)
                return new OperationResult(false, "User not found.");

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

            // Check if user exists
            var user = DatabaseManager.GetUserByID(userID);
            if (user == null)
                return new OperationResult(false, "User not found.");

            // Delete user (permissions will cascade delete)
            bool deleteSuccess = DatabaseManager.DeleteUser(userID);

            if (!deleteSuccess)
                return new OperationResult(false, "Failed to delete user.");

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

            // Get user from database
            var userRecord = DatabaseManager.GetUserByUsername(username);
            
            if (userRecord == null)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: User not found - {username}");
                return (false, null, "Invalid username or password.");
            }

            // Check if user is active
            if (!userRecord.IsActive)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: User disabled - {username}");
                return (false, null, "This account has been disabled.");
            }

            // Verify password using BCrypt
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, userRecord.PasswordHash);
            
            if (!passwordValid)
            {
                AppDebug.Log("adminInstanceManager", $"Authentication failed: Invalid password - {username}");
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
    /// Check if a user has a specific permission
    /// </summary>
    public static bool UserHasPermission(int userID, string permission)
    {
        try
        {
            var permissions = DatabaseManager.GetUserPermissions(userID);
            return permissions.Contains(permission.ToLower());
        }
        catch
        {
            return false;
        }
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
                return new OperationResult(true, "Users already exist.");
            }

            // Create default admin
            var request = new CreateUserRequest(
                Username: "admin",
                Password: "admin", // User should change this immediately
                Permissions: GetDefaultPermissionsForRole("admin"),
                IsActive: true,
                Notes: "Default administrator account - CHANGE PASSWORD IMMEDIATELY"
            );

            var result = CreateUser(request);

            if (result.Success)
            {
                AppDebug.Log("adminInstanceManager", 
                    "Created default admin user (username: admin, password: admin)");
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