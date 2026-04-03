using HawkSyncShared.DTOs;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BHD_ServerManager.Classes.InstanceManagers;

namespace BHD_ServerManager.Classes.SupportClasses
{
    // Static manager that opens the SQLite file on startup and holds an exclusive lock
    // for the lifetime of the process. All database operations must be executed through
    // the provided Execute/ExecuteAsync methods which run against the locked connection.
    //
    // Notes:
    // - Requires the Microsoft.Data.Sqlite NuGet package.
    // - Initialization will attempt to acquire an exclusive lock for up to `timeout`.
    // - While the exclusive lock is held, other processes cannot read or write the DB.
    // - Call Shutdown() on application exit to commit and release the lock.
    public static class DatabaseManager
    {
        private static string databaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");
        private static SqliteConnection? _connection;
        private static string _databasePath = Path.Combine(databaseDirectory, "database.sqlite");
        private static string _schemaFilePath = Path.Combine(databaseDirectory, "database.sqlite.sql");
        
        // Current schema version - increment this when you update database.sqlite.sql
        private const int CURRENT_SCHEMA_VERSION = 3;

        public static bool IsInitialized => _connection != null;

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            string databasePath = _databasePath;

            if (!File.Exists(databasePath))
            {
                AppDebug.Log($"Failed to locate the database file {databasePath}", AppDebug.LogLevel.Error);
                throw new FileNotFoundException("Database file not found.", databasePath);
            }

            var csb = new SqliteConnectionStringBuilder
            {
                DataSource = databasePath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Private
            };

            _connection = new SqliteConnection(csb.ToString());
            _connection.Open();
            
            // Check and upgrade database schema if needed
            UpgradeDatabase();
        }
       
        /*
         * DATABASE MIGRATION SYSTEM (SQL File-Based)
         * 
         * This system automatically upgrades the database schema from the database.sqlite.sql file.
         * The SQL file is the single source of truth for the database schema.
         * 
         * HOW IT WORKS:
         * 
         * 1. On startup, the system compares the current database schema version
         * 2. If the version is outdated, it reads database.sqlite.sql
         * 3. Extracts CREATE TABLE and CREATE INDEX statements
         * 4. Applies them (CREATE IF NOT EXISTS ensures no errors on existing objects)
         * 5. For existing tables, detects and adds missing columns
         * 
         * HOW TO UPGRADE THE DATABASE:
         * 
         * 1. Update the database.sqlite.sql file with your schema changes:
         *    - Add new tables
         *    - Add new columns to existing tables (will auto-detect)
         *    - Add new indexes
         *    - Use CREATE TABLE IF NOT EXISTS and CREATE INDEX IF NOT EXISTS
         * 
         * 2. Increment CURRENT_SCHEMA_VERSION in this file
         * 
         * 3. The migration will automatically apply on next startup
         * 
         * SUPPORTED CHANGES:
         * - ✅ Creating new tables
         * - ✅ Adding new columns to existing tables (with DEFAULT values)
         * - ✅ Creating new indexes
         * - ⚠️ Column renames require manual migration in ApplyCustomMigrations()
         * - ⚠️ Column type changes require manual migration in ApplyCustomMigrations()
         * - ⚠️ Removing columns requires manual migration in ApplyCustomMigrations()
         * 
         * MANUAL MIGRATIONS:
         * If you need complex migrations (rename columns, change types, etc.),
         * add them to the ApplyCustomMigrations() method for the specific version.
         * 
         * NOTES:
         * - Backup is automatically created before applying migrations
         * - All changes run within a transaction and rollback on failure
         * - The system preserves all existing data
         */

        /// <summary>
        /// Main upgrade method that reads database.sqlite.sql and applies schema changes
        /// </summary>
        private static void UpgradeDatabase()
        {
            try
            {               
                using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
                conn.Open();

                // Get current schema version (default to 0 for legacy databases)
                int currentVersion = GetSchemaVersion(conn);

                if (currentVersion < CURRENT_SCHEMA_VERSION)
                {
                    
                    // Create a backup before upgrading
                    CreateDatabaseBackup();
                    
                    // Apply schema from SQL file
                    using var tx = conn.BeginTransaction();
                    try
                    {
                        // Apply schema from database.sqlite.sql
                        ApplySchemaFromSqlFile(conn, tx);
                        
                        // Apply any custom migrations that can't be handled automatically
                        ApplyCustomMigrations(conn, tx, currentVersion, CURRENT_SCHEMA_VERSION);
                        
                        // Update schema version
                        SetSchemaVersion(conn, tx, CURRENT_SCHEMA_VERSION);
                        
                        // Commit
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        AppDebug.Log($"ERROR: Database upgrade failed, rolled backed.", AppDebug.LogLevel.Error, ex);
                        throw new Exception($"Database upgrade failed: {ex.Message}, rolled backed.", ex);
                    }
                }
                else if (currentVersion == CURRENT_SCHEMA_VERSION)
                {
                    AppDebug.Log("Database schema is up to date", AppDebug.LogLevel.Info);
                }
                else
                {
                    AppDebug.Log($"WARNING: Database schema version ({currentVersion}) is newer than expected ({CURRENT_SCHEMA_VERSION})", AppDebug.LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error during database upgrade", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Read and apply schema from database.sqlite.sql file
        /// </summary>
        private static void ApplySchemaFromSqlFile(SqliteConnection conn, SqliteTransaction tx)
        {
            if (!File.Exists(_schemaFilePath))
            {
                AppDebug.Log($"Schema file not found: {_schemaFilePath}", AppDebug.LogLevel.Error);
                throw new FileNotFoundException("Database schema file not found", _schemaFilePath);
            }

            string sqlContent = File.ReadAllText(_schemaFilePath);

            // Parse and execute SQL statements
            var statements = ParseSqlStatements(sqlContent);
            
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;

            int tableCount = 0;
            int indexCount = 0;

            foreach (var statement in statements)
            {
                var trimmed = statement.Trim();
                
                // Skip empty statements, comments, and data inserts (INSERT statements)
                if (string.IsNullOrWhiteSpace(trimmed) || 
                    trimmed.StartsWith("--") || 
                    trimmed.StartsWith("/*") ||
                    trimmed.StartsWith("BEGIN TRANSACTION", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("COMMIT", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("INSERT INTO", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                try
                {
                    if (trimmed.StartsWith("CREATE TABLE", StringComparison.OrdinalIgnoreCase))
                    {
                        // Extract table name
                        string tableName = ExtractTableName(trimmed);
                        
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            // Check if table exists
                            if (TableExists(conn, tx, tableName))
                            {
                                AddMissingColumns(conn, tx, tableName, trimmed);
                            }
                            else
                            {
                                cmd.CommandText = trimmed;
                                cmd.ExecuteNonQuery();
                                tableCount++;
                            }
                        }
                    }
                    else if (trimmed.StartsWith("CREATE INDEX", StringComparison.OrdinalIgnoreCase))
                    {
                        // Indexes use IF NOT EXISTS, so safe to execute
                        cmd.CommandText = trimmed;
                        cmd.ExecuteNonQuery();
                        indexCount++;
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log($"Warning: Failed to execute statement", AppDebug.LogLevel.Error, ex);
                    AppDebug.Log($"Statement: {trimmed.Substring(0, Math.Min(100, trimmed.Length))}...", AppDebug.LogLevel.Error);
                    // Continue with other statements - some may fail if already applied
                }
            }

            AppDebug.Log($"Schema application complete: {tableCount} new tables, {indexCount} indexes applied", AppDebug.LogLevel.Info);
        }

        /// <summary>
        /// Parse SQL file into individual statements
        /// </summary>
        private static List<string> ParseSqlStatements(string sqlContent)
        {
            var statements = new List<string>();
            var currentStatement = new StringBuilder();
            bool inQuote = false;
            bool inDoubleQuote = false;

            for (int i = 0; i < sqlContent.Length; i++)
            {
                char c = sqlContent[i];

                // Track quotes to avoid splitting on semicolons inside strings
                if (c == '\'' && (i == 0 || sqlContent[i - 1] != '\\'))
                {
                    inQuote = !inQuote;
                }
                else if (c == '"' && (i == 0 || sqlContent[i - 1] != '\\'))
                {
                    inDoubleQuote = !inDoubleQuote;
                }

                currentStatement.Append(c);

                // Split on semicolon when not inside quotes
                if (c == ';' && !inQuote && !inDoubleQuote)
                {
                    string stmt = currentStatement.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(stmt))
                    {
                        statements.Add(stmt);
                    }
                    currentStatement.Clear();
                }
            }

            // Add any remaining statement
            string lastStmt = currentStatement.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(lastStmt))
            {
                statements.Add(lastStmt);
            }

            return statements;
        }

        /// <summary>
        /// Extract table name from CREATE TABLE statement
        /// </summary>
        private static string ExtractTableName(string createTableStatement)
        {
            try
            {
                // Pattern: CREATE TABLE [IF NOT EXISTS] "tableName" or tableName
                var match = System.Text.RegularExpressions.Regex.Match(
                    createTableStatement,
                    @"CREATE\s+TABLE\s+(?:IF\s+NOT\s+EXISTS\s+)?[""']?(\w+)[""']?",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error extracting table name", AppDebug.LogLevel.Error, ex);
            }

            return string.Empty;
        }

        /// <summary>
        /// Parse column definitions from CREATE TABLE statement
        /// </summary>
        private static List<(string Name, string Definition)> ParseColumnDefinitions(string createTableStatement)
        {
            var columns = new List<(string, string)>();

            try
            {
                // Find the column definitions between parentheses
                int startParen = createTableStatement.IndexOf('(');
                int endParen = createTableStatement.LastIndexOf(')');

                if (startParen == -1 || endParen == -1 || endParen <= startParen)
                    return columns;

                string columnSection = createTableStatement.Substring(startParen + 1, endParen - startParen - 1);

                // Split by comma, but be careful of parentheses (like CHECK constraints)
                var columnDefs = SplitColumnDefinitions(columnSection);

                foreach (var def in columnDefs)
                {
                    var trimmed = def.Trim();
                    
                    // Skip constraints (PRIMARY KEY, FOREIGN KEY, CHECK, UNIQUE)
                    if (trimmed.StartsWith("PRIMARY KEY", StringComparison.OrdinalIgnoreCase) ||
                        trimmed.StartsWith("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                        trimmed.StartsWith("CHECK", StringComparison.OrdinalIgnoreCase) ||
                        trimmed.StartsWith("UNIQUE", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // Extract column name (first token, removing quotes)
                    var tokens = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length > 0)
                    {
                        string colName = tokens[0].Trim('"', '\'');
                        columns.Add((colName, trimmed));
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error parsing column definitions", AppDebug.LogLevel.Error, ex);
            }

            return columns;
        }

        /// <summary>
        /// Split column definitions respecting parentheses
        /// </summary>
        private static List<string> SplitColumnDefinitions(string columnSection)
        {
            var definitions = new List<string>();
            var current = new StringBuilder();
            int parenDepth = 0;

            foreach (char c in columnSection)
            {
                if (c == '(')
                {
                    parenDepth++;
                    current.Append(c);
                }
                else if (c == ')')
                {
                    parenDepth--;
                    current.Append(c);
                }
                else if (c == ',' && parenDepth == 0)
                {
                    definitions.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0)
            {
                definitions.Add(current.ToString());
            }

            return definitions;
        }

        /// <summary>
        /// Check existing table and add any missing columns from the schema
        /// </summary>
        private static void AddMissingColumns(SqliteConnection conn, SqliteTransaction tx, string tableName, string createTableStatement)
        {
            try
            {
                // Get existing columns
                var existingColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = $"PRAGMA table_info({tableName});";
                    
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        existingColumns.Add(reader.GetString(1)); // Column name is at index 1
                    }
                }

                // Parse target columns from CREATE TABLE statement
                var targetColumns = ParseColumnDefinitions(createTableStatement);

                // Find missing columns
                var missingColumns = targetColumns.Where(col => !existingColumns.Contains(col.Name)).ToList();

                if (missingColumns.Count > 0)
                {
                    AppDebug.Log($"Found {missingColumns.Count} missing columns in '{tableName}'", AppDebug.LogLevel.Info);

                    using var cmd = conn.CreateCommand();
                    cmd.Transaction = tx;

                    foreach (var (colName, colDef) in missingColumns)
                    {
                        try
                        {
                            // SQLite ALTER TABLE ADD COLUMN has limitations:
                            // - Column must have a DEFAULT value or be nullable
                            // - Cannot add PRIMARY KEY columns
                            
                            if (colDef.Contains("PRIMARY KEY", StringComparison.OrdinalIgnoreCase))
                            {
                                AppDebug.Log($"Skipping PRIMARY KEY column '{colName}' - cannot add via ALTER TABLE", AppDebug.LogLevel.Info);
                                continue;
                            }

                            cmd.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {colDef};";
                            cmd.ExecuteNonQuery();
                            AppDebug.Log($"Added column '{colName}' to '{tableName}'", AppDebug.LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log($"Warning: Could not add column '{colName}'", AppDebug.LogLevel.Error,ex);
                        }
                    }
                }
                else
                {
                    AppDebug.Log($"Table '{tableName}' is up to date - no missing columns", AppDebug.LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error checking columns for table '{tableName}'", AppDebug.LogLevel.Error, ex);
            }
        }

        /// <summary>
        /// Apply custom migrations that cannot be handled automatically
        /// Add version-specific complex migrations here (column renames, type changes, etc.)
        /// </summary>
        private static void ApplyCustomMigrations(SqliteConnection conn, SqliteTransaction tx, int fromVersion, int toVersion)
        {
            // Apply each version's custom migrations
            for (int version = fromVersion + 1; version <= toVersion; version++)
            {
                switch (version)
                {
                    case 1:
                        // Version 1: No custom migrations needed
                        // All changes handled by SQL file
                        AppDebug.Log("Version 1: No custom migrations needed", AppDebug.LogLevel.Info);
                        break;

                    case 2:
                        AppDebug.Log("Version 2: No custom migrations needed", AppDebug.LogLevel.Info);
                        break;

                    case 3:
                        MigrateBabstatsServersFromSettings(conn, tx);
                        break;

                    default:
                        // No custom migrations for this version
                        break;
                }
            }
        }

        private static void MigrateBabstatsServersFromSettings(SqliteConnection conn, SqliteTransaction tx)
        {
            using var existsCmd = conn.CreateCommand();
            existsCmd.Transaction = tx;
            existsCmd.CommandText = "SELECT COUNT(1) FROM tb_babstatsServers;";

            long existingCount = Convert.ToInt64(existsCmd.ExecuteScalar() ?? 0L);
            if (existingCount > 0)
            {
                AppDebug.Log("Version 3 migration skipped: tb_babstatsServers already contains data", AppDebug.LogLevel.Info);
                return;
            }

            static string GetLegacySetting(SqliteConnection c, SqliteTransaction t, string key, string defaultValue)
            {
                using var cmd = c.CreateCommand();
                cmd.Transaction = t;
                cmd.CommandText = "SELECT value FROM tb_settings WHERE key = $key LIMIT 1;";
                cmd.Parameters.AddWithValue("$key", key);

                object? result = cmd.ExecuteScalar();
                return result?.ToString() ?? defaultValue;
            }

            string profileId = GetLegacySetting(conn, tx, "WebStatsProfileID", string.Empty).Trim();
            string serverPath = GetLegacySetting(conn, tx, "WebStatsServerPath", string.Empty).Trim();
            bool isEnabled = bool.TryParse(GetLegacySetting(conn, tx, "WebStatsEnabled", "false"), out bool parsedEnabled) && parsedEnabled;
            bool announcements = bool.TryParse(GetLegacySetting(conn, tx, "WebStatsAnnouncements", "false"), out bool parsedAnnouncements) && parsedAnnouncements;
            int reportInterval = int.TryParse(GetLegacySetting(conn, tx, "WebStatsReportInterval", "300"), out int parsedReport) ? parsedReport : 300;
            int updateInterval = int.TryParse(GetLegacySetting(conn, tx, "WebStatsUpdateInterval", "60"), out int parsedUpdate) ? parsedUpdate : 60;

            if (string.IsNullOrWhiteSpace(serverPath) && string.IsNullOrWhiteSpace(profileId) && !isEnabled)
            {
                AppDebug.Log("Version 3 migration: No legacy WebStats values to seed", AppDebug.LogLevel.Info);
                return;
            }

            using var insertCmd = conn.CreateCommand();
            insertCmd.Transaction = tx;
            insertCmd.CommandText = @"
                INSERT INTO tb_babstatsServers
                (DisplayName, ServerPath, ProfileID, IsEnabled, EnableAnnouncements, ReportIntervalSeconds, UpdateIntervalSeconds, SortOrder, CreatedUtc, UpdatedUtc)
                VALUES
                ($displayName, $serverPath, $profileId, $isEnabled, $announcements, $reportInterval, $updateInterval, 0, datetime('now'), datetime('now'));
            ";
            insertCmd.Parameters.AddWithValue("$displayName", "Primary");
            insertCmd.Parameters.AddWithValue("$serverPath", serverPath);
            insertCmd.Parameters.AddWithValue("$profileId", profileId.ToUpperInvariant());
            insertCmd.Parameters.AddWithValue("$isEnabled", isEnabled ? 1 : 0);
            insertCmd.Parameters.AddWithValue("$announcements", announcements ? 1 : 0);
            insertCmd.Parameters.AddWithValue("$reportInterval", Math.Clamp(reportInterval, 15, 3600));
            insertCmd.Parameters.AddWithValue("$updateInterval", Math.Clamp(updateInterval, 15, 3600));
            insertCmd.ExecuteNonQuery();

            AppDebug.Log("Version 3 migration: Seeded tb_babstatsServers from legacy WebStats settings", AppDebug.LogLevel.Info);
        }

        /// <summary>
        /// Get the current schema version from the database
        /// </summary>
        private static int GetSchemaVersion(SqliteConnection conn)
        {
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT value FROM tb_settings WHERE key = 'schema_version' LIMIT 1;";
                
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    if (int.TryParse(result.ToString(), out int version))
                    {
                        return version;
                    }
                }
            }
            catch
            {
                // If there's an error (e.g., table doesn't exist), assume version 0
            }
            
            return 0; // Legacy database with no version tracking
        }

        /// <summary>
        /// Set the schema version in the database
        /// </summary>
        private static void SetSchemaVersion(SqliteConnection conn, SqliteTransaction tx, int version)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = @"
                INSERT INTO tb_settings (key, value)
                VALUES ('schema_version', $version)
                ON CONFLICT(key) DO UPDATE SET value = excluded.value;
            ";
            cmd.Parameters.AddWithValue("$version", version.ToString());
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a backup of the database before applying migrations
        /// </summary>
        private static void CreateDatabaseBackup()
        {
            try
            {
                string backupPath = $"{_databasePath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                File.Copy(_databasePath, backupPath, false);
                AppDebug.Log($"Database backup created: {backupPath}", AppDebug.LogLevel.Info);
                
                // Keep only the last 5 backups
                CleanupOldBackups();
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Warning: Failed to create database backup", AppDebug.LogLevel.Error, ex);
                // Don't throw - backup failure shouldn't prevent upgrade if user accepts risk
            }
        }

        /// <summary>
        /// Clean up old backup files, keeping only the most recent ones
        /// </summary>
        private static void CleanupOldBackups()
        {
            try
            {
                var backupFiles = Directory.GetFiles(databaseDirectory, "database.sqlite.backup_*")
                    .OrderByDescending(f => File.GetCreationTime(f))
                    .Skip(5)
                    .ToList();

                foreach (var file in backupFiles)
                {
                    File.Delete(file);
                    AppDebug.Log($"Deleted old backup: {file}", AppDebug.LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Warning: Failed to cleanup old backups", AppDebug.LogLevel.Error, ex);
            }
        }

        /// <summary>
        /// Check if a table exists (helper for migrations)
        /// </summary>
        private static bool TableExists(SqliteConnection conn, SqliteTransaction tx, string tableName)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=$tableName;";
            cmd.Parameters.AddWithValue("$tableName", tableName);
            
            var result = cmd.ExecuteScalar();
            return result != null;
        }

        /// <summary>
        /// Grabs a value for an expected key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetSetting(string key, string defaultValue)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            // Open a short-lived connection. This works because your process owns the exclusive lock.
            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            // 1. Try read
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "SELECT value FROM tb_settings WHERE key = $key LIMIT 1;";
                cmd.Parameters.AddWithValue("$key", key);

                var result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    tx.Commit();
                    AppDebug.Log($"DatabaseManager: GetSetting: ({key} - {result}) Default:({defaultValue})", AppDebug.LogLevel.Info);
                    return (string)result;
                }
            }

            // 2. Not found → Insert default value
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "INSERT INTO tb_settings (key, value) VALUES ($key, $value);";
                cmd.Parameters.AddWithValue("$key", key);
                cmd.Parameters.AddWithValue("$value", defaultValue);
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            return defaultValue;
        }

        /// <summary>
        /// Set a key's value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException"></exception>

        public static void SetSetting(string key, string value)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;

                // INSERT OR REPLACE handles:
                // - updating existing key
                // - inserting new key
                cmd.CommandText = @"
            INSERT INTO tb_settings (key, value)
            VALUES ($key, $value)
            ON CONFLICT(key) DO UPDATE SET value = excluded.value;
        ";

                cmd.Parameters.AddWithValue("$key", key);
                cmd.Parameters.AddWithValue("$value", value);

                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            AppDebug.Log($"DatabaseManager: SetSetting: ({key} - {value})", AppDebug.LogLevel.Info);
        }
        /// <summary>
        /// Returns an array of default maps from the database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<MapObject> GetDefaultMaps()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var maps = new List<MapObject>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT mapID, mapName, mapFile, modType, mapType
                FROM tb_defaultMaps
                ORDER BY mapID;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                maps.Add(new MapObject
                {
                    MapID   = reader.GetInt32(0),
                    MapName = reader.GetString(1),
                    MapFile = reader.GetString(2),
                    ModType = reader.GetInt32(3),
                    MapType = reader.GetInt32(4)
                });
            }

            AppDebug.Log($"DatabaseManager: Loaded {maps.Count} default maps", AppDebug.LogLevel.Info);

            return maps;
        }
        
        /// <summary>
        /// Grab maps from database for XX playlist.
        /// </summary>
        /// <param name="playlistID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<MapObject> GetPlaylistMaps(int playlistID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var maps = new List<MapObject>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT mapID, mapName, mapFile, modType, mapType
                FROM tb_mapPlaylists
                WHERE playlistID = $playlistID
                ORDER BY mapID;
            ";

            cmd.Parameters.AddWithValue("$playlistID", playlistID);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                maps.Add(new MapObject
                {
                    MapID   = reader.GetInt32(0),
                    MapName = reader.GetString(1),
                    MapFile = reader.GetString(2),
                    ModType = reader.GetInt32(3),
                    MapType = reader.GetInt32(4)
                });
            }

            AppDebug.Log($"DatabaseManager: Loaded {maps.Count} maps for playlist {playlistID}", AppDebug.LogLevel.Info);

            return maps;
        }

        public static void SavePlaylist(int playlistID, List<MapObject> maps)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                // 1. Delete existing playlist rows
                using (var deleteCmd = conn.CreateCommand())
                {
                    deleteCmd.Transaction = tx;
                    deleteCmd.CommandText = @"
                        DELETE FROM tb_mapPlaylists
                        WHERE playlistID = $playlistID;
                    ";
                    deleteCmd.Parameters.AddWithValue("$playlistID", playlistID);
                    deleteCmd.ExecuteNonQuery();
                }

                // 2. Insert new rows
                using (var insertCmd = conn.CreateCommand())
                {
                    insertCmd.Transaction = tx;
                    insertCmd.CommandText = @"
                        INSERT INTO tb_mapPlaylists
                        (playlistID, mapID, mapName, mapFile, modType, mapType)
                        VALUES
                        ($playlistID, $mapID, $mapName, $mapFile, $modType, $mapType);
                    ";

                    foreach (var map in maps)
                    {
                        insertCmd.Parameters.Clear();

                        insertCmd.Parameters.AddWithValue("$playlistID", playlistID);
                        insertCmd.Parameters.AddWithValue("$mapID",   map.MapID);
                        insertCmd.Parameters.AddWithValue("$mapName", map.MapName);
                        insertCmd.Parameters.AddWithValue("$mapFile", map.MapFile);
                        insertCmd.Parameters.AddWithValue("$modType", map.ModType);
                        insertCmd.Parameters.AddWithValue("$mapType", map.MapType);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                tx.Commit();

                AppDebug.Log($"DatabaseManager: Saved playlist {playlistID} ({maps.Count} maps)", AppDebug.LogLevel.Info);
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Load all slap messages from the database.
        /// </summary>
        public static List<SlapMessages> GetSlapMessages()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var messages = new List<SlapMessages>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT slapMessageId, slapMessageText
                FROM tb_chatSlapMessages
                ORDER BY slapMessageText COLLATE NOCASE;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new SlapMessages
                {
                    SlapMessageId = reader.GetInt32(0),
                    SlapMessageText = reader.GetString(1)
                });
            }

            AppDebug.Log($"Loaded {messages.Count} slap messages", AppDebug.LogLevel.Info);
            return messages;
        }

        /// <summary>
        /// Add a new slap message to the database.
        /// </summary>
        public static int AddSlapMessage(string messageText)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            if (string.IsNullOrWhiteSpace(messageText))
                throw new ArgumentException("Message text cannot be empty.", nameof(messageText));

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_chatSlapMessages (slapMessageText)
                    VALUES ($messageText);
                    SELECT last_insert_rowid();
                ";
                cmd.Parameters.AddWithValue("$messageText", messageText);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added slap message: {messageText} (ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Remove a slap message from the database.
        /// </summary>
        public static bool RemoveSlapMessage(int slapMessageId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_chatSlapMessages
                    WHERE slapMessageId = $id;
                ";
                cmd.Parameters.AddWithValue("$id", slapMessageId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Removed slap message ID: {slapMessageId}", AppDebug.LogLevel.Info);

                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Load all auto messages from the database.
        /// </summary>
        public static List<AutoMessages> GetAutoMessages()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var messages = new List<AutoMessages>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT autoMessageId, autoMessageText, autoMessageTrigger
                FROM tb_chatAutoMessages
                ORDER BY autoMessageTrigger, autoMessageId;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new AutoMessages
                {
                    AutoMessageId = reader.GetInt32(0),
                    AutoMessageText = reader.GetString(1),
                    AutoMessageTigger = reader.GetInt32(2)
                });
            }

            AppDebug.Log($"Loaded {messages.Count} auto messages", AppDebug.LogLevel.Info);
            return messages;
        }

        /// <summary>
        /// Add a new auto message to the database.
        /// </summary>
        public static int AddAutoMessage(string messageText, int triggerSeconds)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            if (string.IsNullOrWhiteSpace(messageText))
                throw new ArgumentException("Message text cannot be empty.", nameof(messageText));

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_chatAutoMessages (autoMessageText, autoMessageTrigger)
                    VALUES ($messageText, $trigger);
                    SELECT last_insert_rowid();
                ";
                cmd.Parameters.AddWithValue("$messageText", messageText);
                cmd.Parameters.AddWithValue("$trigger", triggerSeconds);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added auto message: {messageText} (Trigger: {triggerSeconds}s, ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Remove an auto message from the database.
        /// </summary>
        public static bool RemoveAutoMessage(int autoMessageId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_chatAutoMessages
                    WHERE autoMessageId = $id;
                ";
                cmd.Parameters.AddWithValue("$id", autoMessageId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Removed auto message ID: {autoMessageId}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Save a chat log entry to the database.
        /// </summary>
        public static void SaveChatLog(ChatLogObject chatLog)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_chatLogs 
                    (messageTimeStamp, playerName, messageType, messageType2, messageText)
                    VALUES 
                    ($timestamp, $playerName, $type, $type2, $text);
                ";

                // Store as Unix timestamp (INTEGER) for better performance
                long unixTimestamp = ((DateTimeOffset)chatLog.MessageTimeStamp).ToUnixTimeSeconds();
                cmd.Parameters.AddWithValue("$timestamp", unixTimestamp);
                cmd.Parameters.AddWithValue("$playerName", chatLog.PlayerName);
                cmd.Parameters.AddWithValue("$type", chatLog.MessageType);
                cmd.Parameters.AddWithValue("$type2", chatLog.MessageType2);
                cmd.Parameters.AddWithValue("$text", chatLog.MessageText);

                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Load chat logs from the database with optional date filtering.
        /// </summary>
        public static List<ChatLogObject> GetChatLogs(DateTime? startDate = null, DateTime? endDate = null, int limit = 1000)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var logs = new List<ChatLogObject>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
    
            var query = new StringBuilder(@"
                SELECT messageTimeStamp, playerName, messageType, messageType2, messageText
                FROM tb_chatLogs
            ");

            var conditions = new List<string>();
    
            if (startDate.HasValue)
            {
                conditions.Add("messageTimeStamp >= $startDate");
                cmd.Parameters.AddWithValue("$startDate", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (endDate.HasValue)
            {
                conditions.Add("messageTimeStamp <= $endDate");
                cmd.Parameters.AddWithValue("$endDate", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (conditions.Count > 0)
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", conditions));
            }

            query.Append(" ORDER BY messageTimeStamp DESC");
            query.Append(" LIMIT $limit;");
    
            cmd.Parameters.AddWithValue("$limit", limit);
            cmd.CommandText = query.ToString();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                logs.Add(new ChatLogObject
                {
                    MessageTimeStamp = DateTime.Parse(reader.GetString(0)),
                    PlayerName = reader.GetString(1),
                    MessageType = reader.GetInt32(2),
                    MessageType2 = reader.GetInt32(3),
                    MessageText = reader.GetString(4)
                });
            }

            AppDebug.Log($"Loaded {logs.Count} chat log entries", AppDebug.LogLevel.Info);
            return logs;
        }

        /// <summary>
        /// Safely read timestamp from database, handling both INTEGER (Unix) and TEXT (legacy) formats.
        /// </summary>
        private static DateTime SafeReadTimestamp(SqliteDataReader reader, int columnIndex)
        {
            try
            {
                // Try to read as INTEGER (Unix timestamp) first
                if (reader.GetFieldType(columnIndex) == typeof(long))
                {
                    long unixTimestamp = reader.GetInt64(columnIndex);
                    
                    // Validate it's a reasonable timestamp (2020-2040)
                    if (unixTimestamp >= 1577836800 && unixTimestamp <= 2209017600)
                    {
                        return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                    }
                    else
                    {
                        AppDebug.Log($"Warning: Invalid Unix timestamp {unixTimestamp}, attempting TEXT parse", AppDebug.LogLevel.Warning);
                    }
                }
            }
            catch (InvalidCastException)
            {
                // Column is TEXT, fall through to string parsing
                AppDebug.Log($"Info: Timestamp column is not INTEGER, attempting to read as TEXT", AppDebug.LogLevel.Info);
            }

            try
            {
                // Fallback: Try to read as TEXT (legacy format)
                string textTimestamp = reader.GetString(columnIndex);
                
                if (DateTime.TryParse(textTimestamp, out DateTime parsedDate))
                {
                    AppDebug.Log($"Warning: Found TEXT timestamp (legacy format): {textTimestamp}", AppDebug.LogLevel.Info);
                    return parsedDate;
                }
                else
                {
                    AppDebug.Log($"Error: Failed to parse timestamp: {textTimestamp}", AppDebug.LogLevel.Info);
                    return DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error reading timestamp: {ex.Message}", AppDebug.LogLevel.Error);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Get distinct player names for filter dropdown.
        /// </summary>
        public static List<string> GetDistinctPlayerNames(int limit = 500)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var players = new List<string>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT DISTINCT playerName 
                FROM tb_chatLogs 
                ORDER BY playerName ASC 
                LIMIT $limit;
            ";
            cmd.Parameters.AddWithValue("$limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                players.Add(reader.GetString(0));
            }

            AppDebug.Log($"Loaded {players.Count} distinct player names", AppDebug.LogLevel.Info);
            return players;
        }

        /// <summary>
        /// Get paginated chat logs with advanced filtering.
        /// </summary>
        public static (List<ChatLogObject> logs, int totalCount) GetChatLogsFiltered(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? playerNameFilter = null,
            int? messageTypeFilter = null,
            int? teamFilter = null,
            string? searchText = null,
            int page = 1,
            int pageSize = 100)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var logs = new List<ChatLogObject>();
            int totalCount = 0;

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            // Build WHERE clause
            var conditions = new List<string>();
            
            if (startDate.HasValue)
            {
                // Support both INTEGER (Unix) and TEXT formats
                long unixStart = ((DateTimeOffset)startDate.Value).ToUnixTimeSeconds();
                string textStart = startDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                conditions.Add($"(messageTimeStamp >= {unixStart} OR messageTimeStamp >= '{textStart}')");
            }
            
            if (endDate.HasValue)
            {
                long unixEnd = ((DateTimeOffset)endDate.Value).ToUnixTimeSeconds();
                string textEnd = endDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                conditions.Add($"(messageTimeStamp <= {unixEnd} OR messageTimeStamp <= '{textEnd}')");
            }
            
            if (!string.IsNullOrEmpty(playerNameFilter))
                conditions.Add("playerName = $playerName");

            if (messageTypeFilter.HasValue)
                conditions.Add("messageType = $messageType");

            if (teamFilter.HasValue)
                conditions.Add("messageType2 = $teamFilter");

            if (!string.IsNullOrEmpty(searchText))
                conditions.Add("messageText LIKE $searchText");

            string whereClause = conditions.Count > 0 
                ? " WHERE " + string.Join(" AND ", conditions) 
                : "";

            // Get total count
            using (var countCmd = conn.CreateCommand())
            {
                countCmd.CommandText = $"SELECT COUNT(*) FROM tb_chatLogs{whereClause};";

                if (!string.IsNullOrEmpty(playerNameFilter))
                    countCmd.Parameters.AddWithValue("$playerName", playerNameFilter);
                if (messageTypeFilter.HasValue)
                    countCmd.Parameters.AddWithValue("$messageType", messageTypeFilter.Value);
                if (teamFilter.HasValue)
                    countCmd.Parameters.AddWithValue("$teamFilter", teamFilter.Value);
                if (!string.IsNullOrEmpty(searchText))
                    countCmd.Parameters.AddWithValue("$searchText", $"%{searchText}%");

                totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            // Get paginated data
            using (var dataCmd = conn.CreateCommand())
            {
                int offset = (page - 1) * pageSize;
                dataCmd.CommandText = $@"
                    SELECT messageTimeStamp, playerName, messageType, messageType2, messageText
                    FROM tb_chatLogs
                    {whereClause}
                    ORDER BY messageTimeStamp DESC
                    LIMIT $limit OFFSET $offset;
                ";

                if (!string.IsNullOrEmpty(playerNameFilter))
                    dataCmd.Parameters.AddWithValue("$playerName", playerNameFilter);
                if (messageTypeFilter.HasValue)
                    dataCmd.Parameters.AddWithValue("$messageType", messageTypeFilter.Value);
                if (teamFilter.HasValue)
                    dataCmd.Parameters.AddWithValue("$teamFilter", teamFilter.Value);
                if (!string.IsNullOrEmpty(searchText))
                    dataCmd.Parameters.AddWithValue("$searchText", $"%{searchText}%");

                dataCmd.Parameters.AddWithValue("$limit", pageSize);
                dataCmd.Parameters.AddWithValue("$offset", offset);

                using var reader = dataCmd.ExecuteReader();
                while (reader.Read())
                {
                    int msgType = reader.GetInt32(2);
                    int teamNum = reader.GetInt32(3);

                    // Use safe reader that handles both INTEGER and TEXT
                    DateTime timestamp = SafeReadTimestamp(reader, 0);

                    logs.Add(new ChatLogObject
                    {
                        MessageTimeStamp = timestamp,
                        PlayerName = reader.GetString(1),
                        MessageType = msgType,
                        MessageType2 = teamNum,
                        MessageText = reader.GetString(4),
                        TeamDisplay = chatInstanceManager.GetTeamDisplayName(msgType, teamNum)
                    });
                }
            }

            AppDebug.Log($"Loaded {logs.Count} of {totalCount} chat logs (Page {page})", AppDebug.LogLevel.Info);
            return (logs, totalCount);
        }

        /// <summary>
        /// Migrate messageTimeStamp from TEXT to INTEGER (Unix timestamp) with validation.
        /// </summary>
        public static void MigrateChatLogsTimestamps()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                // Check if already migrated
                bool isAlreadyInteger = false;
                using (var checkCmd = conn.CreateCommand())
                {
                    checkCmd.Transaction = tx;
                    checkCmd.CommandText = "PRAGMA table_info(tb_chatLogs);";
                    using var reader = checkCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "messageTimeStamp")
                        {
                            string columnType = reader.GetString(2);
                            if (columnType == "INTEGER")
                            {
                                isAlreadyInteger = true;
                                AppDebug.Log("Chat logs timestamps already migrated to INTEGER", AppDebug.LogLevel.Info);
                                break;
                            }
                        }
                    }
                }

                if (isAlreadyInteger)
                {
                    tx.Commit();
                    return;
                }

                // Count existing records
                int originalCount = 0;
                using (var countCmd = conn.CreateCommand())
                {
                    countCmd.Transaction = tx;
                    countCmd.CommandText = "SELECT COUNT(*) FROM tb_chatLogs;";
                    originalCount = Convert.ToInt32(countCmd.ExecuteScalar());
                }

                AppDebug.Log($"Starting migration of {originalCount} chat log records...", AppDebug.LogLevel.Info);

                // Create new table with INTEGER timestamp
                using (var createCmd = conn.CreateCommand())
                {
                    createCmd.Transaction = tx;
                    createCmd.CommandText = @"
                        CREATE TABLE tb_chatLogs_new (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            messageTimeStamp INTEGER NOT NULL,
                            playerName TEXT NOT NULL,
                            messageType INTEGER NOT NULL,
                            messageType2 INTEGER NOT NULL,
                            messageText TEXT NOT NULL
                        );
                    ";
                    createCmd.ExecuteNonQuery();
                }

                // Copy data with timestamp conversion
                using (var copyCmd = conn.CreateCommand())
                {
                    copyCmd.Transaction = tx;
                    copyCmd.CommandText = @"
                        INSERT INTO tb_chatLogs_new (id, messageTimeStamp, playerName, messageType, messageType2, messageText)
                        SELECT 
                            id,
                            CAST(strftime('%s', messageTimeStamp) AS INTEGER),
                            playerName,
                            messageType,
                            messageType2,
                            messageText
                        FROM tb_chatLogs
                        WHERE messageTimeStamp IS NOT NULL;
                    ";
                    int rowsAffected = copyCmd.ExecuteNonQuery();
                    
                    if (rowsAffected != originalCount)
                    {
                        throw new Exception($"Migration mismatch: {originalCount} original records, {rowsAffected} migrated");
                    }
                    
                    AppDebug.Log($"Successfully migrated {rowsAffected} records", AppDebug.LogLevel.Info);
                }

                // Validate migration - check a sample record
                using (var validateCmd = conn.CreateCommand())
                {
                    validateCmd.Transaction = tx;
                    validateCmd.CommandText = "SELECT messageTimeStamp FROM tb_chatLogs_new LIMIT 1;";
                    var result = validateCmd.ExecuteScalar();
                    
                    if (result != null && result != DBNull.Value)
                    {
                        long timestamp = Convert.ToInt64(result);
                        // Validate it's a reasonable Unix timestamp (between 2020 and 2040)
                        if (timestamp < 1577836800 || timestamp > 2209017600)
                        {
                            AppDebug.Log($"Error: Invalid timestamp after migration: {timestamp}", AppDebug.LogLevel.Error);
                            throw new Exception($"Invalid timestamp after migration: {timestamp}");
                        }
                        AppDebug.Log($"Validation passed. Sample timestamp: {timestamp}", AppDebug.LogLevel.Info);
                    }
                }

                // Drop old table
                using (var dropCmd = conn.CreateCommand())
                {
                    dropCmd.Transaction = tx;
                    dropCmd.CommandText = "DROP TABLE tb_chatLogs;";
                    dropCmd.ExecuteNonQuery();
                }

                // Rename new table
                using (var renameCmd = conn.CreateCommand())
                {
                    renameCmd.Transaction = tx;
                    renameCmd.CommandText = "ALTER TABLE tb_chatLogs_new RENAME TO tb_chatLogs;";
                    renameCmd.ExecuteNonQuery();
                }

                // Create indexes for performance
                using (var indexCmd = conn.CreateCommand())
                {
                    indexCmd.Transaction = tx;
                    indexCmd.CommandText = @"
                        CREATE INDEX IF NOT EXISTS idx_chatLogs_timestamp ON tb_chatLogs(messageTimeStamp DESC);
                        CREATE INDEX IF NOT EXISTS idx_chatLogs_playerName ON tb_chatLogs(playerName);
                        CREATE INDEX IF NOT EXISTS idx_chatLogs_messageType ON tb_chatLogs(messageType);
                    ";
                    indexCmd.ExecuteNonQuery();
                }

                tx.Commit();
                AppDebug.Log("Chat logs timestamp migration completed successfully", AppDebug.LogLevel.Info);
            }
            catch (Exception ex)
            {
                tx.Rollback();

                AppDebug.Log($"Migration failed", AppDebug.LogLevel.Error, ex);

                throw new Exception($"Chat log migration failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load all proxy records from the database.
        /// </summary>
        public static List<proxyRecord> GetProxyRecords()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var records = new List<proxyRecord>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT RecordID, IPAddress, IsVpn, IsProxy, IsTor, RiskScore, 
                       Provider, CountryCode, City, Region, CacheExpiry, LastChecked
                FROM tb_proxyRecords
                ORDER BY RecordID;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new proxyRecord
                {
                    RecordID = reader.GetInt32(0),
                    IPAddress = IPAddress.Parse(reader.GetString(1)),
                    IsVpn = reader.GetInt32(2) == 1,
                    IsProxy = reader.GetInt32(3) == 1,
                    IsTor = reader.GetInt32(4) == 1,
                    RiskScore = reader.GetInt32(5),
                    Provider = reader.IsDBNull(6) ? null : reader.GetString(6),
                    CountryCode = reader.IsDBNull(7) ? null : reader.GetString(7),
                    City = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Region = reader.IsDBNull(9) ? null : reader.GetString(9),
                    CacheExpiry = DateTime.Parse(reader.GetString(10)),
                    LastChecked = DateTime.Parse(reader.GetString(11))
                });
            }

            AppDebug.Log($"Loaded {records.Count} proxy records", AppDebug.LogLevel.Info);

            return records;
        }

        /// <summary>
        /// Load all player name records for a specific category.
        /// </summary>
        public static List<banInstancePlayerName> GetPlayerNameRecords(RecordCategory category)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var records = new List<banInstancePlayerName>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT RecordID, MatchID, PlayerName, Date, ExpireDate, AssociatedIP, RecordType, RecordCategory, Notes
                FROM tb_playerNameRecords
                WHERE RecordCategory = $category
                ORDER BY RecordID;
            ";
            cmd.Parameters.AddWithValue("$category", (int)category);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new banInstancePlayerName
                {
                    RecordID = reader.GetInt32(0),
                    MatchID = reader.GetInt32(1),
                    PlayerName = reader.GetString(2),
                    Date = DateTime.Parse(reader.GetString(3)),
                    ExpireDate = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
                    AssociatedIP = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    RecordType = (banInstanceRecordType)reader.GetInt32(6),
                    RecordCategory = reader.GetInt32(7),
                    Notes = reader.GetString(8)
                });
            }

            AppDebug.Log($"Loaded {records.Count} player name records for category {category}", AppDebug.LogLevel.Info);
            return records;
        }

        /// <summary>
        /// Load all player IP records for a specific category.
        /// </summary>
        public static List<banInstancePlayerIP> GetPlayerIPRecords(RecordCategory category)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var records = new List<banInstancePlayerIP>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT RecordID, MatchID, PlayerIP, SubnetMask, Date, ExpireDate, AssociatedName, RecordType, RecordCategory, Notes
                FROM tb_playerIPRecords
                WHERE RecordCategory = $category
                ORDER BY RecordID;
            ";
            cmd.Parameters.AddWithValue("$category", (int)category);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new banInstancePlayerIP
                {
                    RecordID = reader.GetInt32(0),
                    MatchID = reader.GetInt32(1),
                    PlayerIP = IPAddress.Parse(reader.GetString(2)),
                    SubnetMask = reader.GetInt32(3),
                    Date = DateTime.Parse(reader.GetString(4)),
                    ExpireDate = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
                    AssociatedName = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    RecordType = (banInstanceRecordType)reader.GetInt32(7),
                    RecordCategory = reader.GetInt32(8),
                    Notes = reader.GetString(9)
                });
            }

            AppDebug.Log($"Loaded {records.Count} player IP records for category {category}", AppDebug.LogLevel.Info);
            return records;
        }

        /// <summary>
        /// Add a new player name record to the database.
        /// </summary>
        public static int AddPlayerNameRecord(banInstancePlayerName record)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_playerNameRecords 
                    (MatchID, PlayerName, Date, ExpireDate, AssociatedIP, RecordType, RecordCategory, Notes)
                    VALUES 
                    ($matchId, $playerName, $date, $expireDate, $associatedIP, $recordType, $recordCategory, $notes);
                    SELECT last_insert_rowid();
                ";

                cmd.Parameters.AddWithValue("$matchId", record.MatchID);
                cmd.Parameters.AddWithValue("$playerName", record.PlayerName);
                cmd.Parameters.AddWithValue("$date", record.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$expireDate", record.ExpireDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                // Convert 0 to NULL for foreign key
                cmd.Parameters.AddWithValue("$associatedIP", record.AssociatedIP > 0 ? (object)record.AssociatedIP : DBNull.Value);
                cmd.Parameters.AddWithValue("$recordType", (int)record.RecordType);
                cmd.Parameters.AddWithValue("$recordCategory", record.RecordCategory);
                cmd.Parameters.AddWithValue("$notes", record.Notes);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added player name record: {record.PlayerName} (ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch
            {
                tx.Rollback();
                AppDebug.Log($"Failed to add player name record: {record.PlayerName}", AppDebug.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Add a new player IP record to the database.
        /// </summary>
        public static int AddPlayerIPRecord(banInstancePlayerIP record)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_playerIPRecords 
                    (MatchID, PlayerIP, SubnetMask, Date, ExpireDate, AssociatedName, RecordType, RecordCategory, Notes)
                    VALUES 
                    ($matchId, $playerIP, $subnetMask, $date, $expireDate, $associatedName, $recordType, $recordCategory, $notes);
                    SELECT last_insert_rowid();
                ";

                cmd.Parameters.AddWithValue("$matchId", record.MatchID);
                cmd.Parameters.AddWithValue("$playerIP", record.PlayerIP.ToString());
                cmd.Parameters.AddWithValue("$subnetMask", record.SubnetMask);
                cmd.Parameters.AddWithValue("$date", record.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$expireDate", record.ExpireDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                // Convert 0 to NULL for foreign key
                cmd.Parameters.AddWithValue("$associatedName", record.AssociatedName > 0 ? (object)record.AssociatedName : DBNull.Value);
                cmd.Parameters.AddWithValue("$recordType", (int)record.RecordType);
                cmd.Parameters.AddWithValue("$recordCategory", record.RecordCategory);
                cmd.Parameters.AddWithValue("$notes", record.Notes);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added player IP record: {record.PlayerIP}/{record.SubnetMask} (ID: {newId})", AppDebug.LogLevel.Info);

                return (int)newId;
            }
            catch
            {
                tx.Rollback();
                AppDebug.Log($"Failed to add player IP record: {record.PlayerIP}/{record.SubnetMask}", AppDebug.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Update an existing player name record.
        /// </summary>
        public static bool UpdatePlayerNameRecord(banInstancePlayerName record)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_playerNameRecords 
                    SET MatchID = $matchId,
                        PlayerName = $playerName,
                        Date = $date,
                        ExpireDate = $expireDate,
                        AssociatedIP = $associatedIP,
                        RecordType = $recordType,
                        RecordCategory = $recordCategory,
                        Notes = $notes
                    WHERE RecordID = $recordId;
                ";

                cmd.Parameters.AddWithValue("$recordId", record.RecordID);
                cmd.Parameters.AddWithValue("$matchId", record.MatchID);
                cmd.Parameters.AddWithValue("$playerName", record.PlayerName);
                cmd.Parameters.AddWithValue("$date", record.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$expireDate", record.ExpireDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                // Convert 0 to NULL for foreign key
                cmd.Parameters.AddWithValue("$associatedIP", record.AssociatedIP > 0 ? (object)record.AssociatedIP : DBNull.Value);
                cmd.Parameters.AddWithValue("$recordType", (int)record.RecordType);
                cmd.Parameters.AddWithValue("$recordCategory", record.RecordCategory);
                cmd.Parameters.AddWithValue("$notes", record.Notes);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Updated player name record ID: {record.RecordID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Update an existing player IP record.
        /// </summary>
        public static bool UpdatePlayerIPRecord(banInstancePlayerIP record)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_playerIPRecords 
                    SET MatchID = $matchId,
                        PlayerIP = $playerIP,
                        SubnetMask = $subnetMask,
                        Date = $date,
                        ExpireDate = $expireDate,
                        AssociatedName = $associatedName,
                        RecordType = $recordType,
                        RecordCategory = $recordCategory,
                        Notes = $notes
                    WHERE RecordID = $recordId;
                ";

                cmd.Parameters.AddWithValue("$recordId", record.RecordID);
                cmd.Parameters.AddWithValue("$matchId", record.MatchID);
                cmd.Parameters.AddWithValue("$playerIP", record.PlayerIP.ToString());
                cmd.Parameters.AddWithValue("$subnetMask", record.SubnetMask);
                cmd.Parameters.AddWithValue("$date", record.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$expireDate", record.ExpireDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                // Convert 0 to NULL for foreign key
                cmd.Parameters.AddWithValue("$associatedName", record.AssociatedName > 0 ? (object)record.AssociatedName : DBNull.Value);
                cmd.Parameters.AddWithValue("$recordType", (int)record.RecordType);
                cmd.Parameters.AddWithValue("$recordCategory", record.RecordCategory);
                cmd.Parameters.AddWithValue("$notes", record.Notes);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Updated player IP record ID: {record.RecordID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Remove a player name record from the database.
        /// </summary>
        public static bool RemovePlayerNameRecord(int recordId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_playerNameRecords
                    WHERE RecordID = $recordId;
                ";
                cmd.Parameters.AddWithValue("$recordId", recordId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Removed player name record ID: {recordId}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Remove a player IP record from the database.
        /// </summary>
        public static bool RemovePlayerIPRecord(int recordId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_playerIPRecords
                    WHERE RecordID = $recordId;
                ";
                cmd.Parameters.AddWithValue("$recordId", recordId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Removed player IP record ID: {recordId}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Load all proxy blocked countries from the database.
        /// </summary>
        public static List<proxyCountry> GetProxyBlockedCountries()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var countries = new List<proxyCountry>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT RecordID, CountryCode, CountryName
                FROM tb_proxyBlockedCountries
                ORDER BY CountryName COLLATE NOCASE;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                countries.Add(new proxyCountry
                {
                    RecordID = reader.GetInt32(0),
                    CountryCode = reader.GetString(1),
                    CountryName = reader.GetString(2)
                });
            }

            AppDebug.Log($"Loaded {countries.Count} blocked countries", AppDebug.LogLevel.Info);
            return countries;
        }

        /// <summary>
        /// Add a new blocked country to the database.
        /// </summary>
        public static int AddProxyBlockedCountry(string countryCode, string countryName)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
                throw new ArgumentException("Country code must be exactly 2 characters.", nameof(countryCode));

            if (string.IsNullOrWhiteSpace(countryName))
                throw new ArgumentException("Country name cannot be empty.", nameof(countryName));

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_proxyBlockedCountries (CountryCode, CountryName)
                    VALUES ($countryCode, $countryName);
                    SELECT last_insert_rowid();
                ";
                cmd.Parameters.AddWithValue("$countryCode", countryCode.ToUpper());
                cmd.Parameters.AddWithValue("$countryName", countryName);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added blocked country: {countryCode} - {countryName} (ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint
            {
                tx.Rollback();
                AppDebug.Log($"Country code {countryCode} already exists", AppDebug.LogLevel.Error, ex);
                return -1;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to add blocked country: {countryCode} - {countryName}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Remove a blocked country from the database by RecordID.
        /// </summary>
        public static bool RemoveProxyBlockedCountry(int recordId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_proxyBlockedCountries
                    WHERE RecordID = $recordId;
                ";
                cmd.Parameters.AddWithValue("$recordId", recordId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Removed blocked country ID: {recordId}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Add a new proxy record to the database.
        /// </summary>
        public static int AddProxyRecord(proxyRecord record)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_proxyRecords 
                    (IPAddress, IsVpn, IsProxy, IsTor, RiskScore, Provider, CountryCode, City, Region, CacheExpiry, LastChecked)
                    VALUES 
                    ($ipAddress, $isVpn, $isProxy, $isTor, $riskScore, $provider, $countryCode, $city, $region, $cacheExpiry, $lastChecked);
                    SELECT last_insert_rowid();
                ";

                cmd.Parameters.AddWithValue("$ipAddress", record.IPAddress.ToString());
                cmd.Parameters.AddWithValue("$isVpn", record.IsVpn ? 1 : 0);
                cmd.Parameters.AddWithValue("$isProxy", record.IsProxy ? 1 : 0);
                cmd.Parameters.AddWithValue("$isTor", record.IsTor ? 1 : 0);
                cmd.Parameters.AddWithValue("$riskScore", record.RiskScore);
                cmd.Parameters.AddWithValue("$provider", record.Provider ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$countryCode", record.CountryCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$city", record.City ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$region", record.Region ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$cacheExpiry", record.CacheExpiry.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$lastChecked", record.LastChecked.ToString("yyyy-MM-dd HH:mm:ss"));

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added proxy record: {record.IPAddress} (ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint
            {
                tx.Rollback();
                AppDebug.Log($"Proxy record for {record.IPAddress} already exists", AppDebug.LogLevel.Error, ex);
                return -1;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to add proxy record: {record.IPAddress}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Update an existing proxy record.
        /// </summary>
        public static bool UpdateProxyRecord(proxyRecord record)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_proxyRecords 
                    SET IPAddress = $ipAddress,
                        IsVpn = $isVpn,
                        IsProxy = $isProxy,
                        IsTor = $isTor,
                        RiskScore = $riskScore,
                        Provider = $provider,
                        CountryCode = $countryCode,
                        City = $city,
                        Region = $region,
                        CacheExpiry = $cacheExpiry,
                        LastChecked = $lastChecked
                    WHERE RecordID = $recordId;
                ";

                cmd.Parameters.AddWithValue("$recordId", record.RecordID);
                cmd.Parameters.AddWithValue("$ipAddress", record.IPAddress.ToString());
                cmd.Parameters.AddWithValue("$isVpn", record.IsVpn ? 1 : 0);
                cmd.Parameters.AddWithValue("$isProxy", record.IsProxy ? 1 : 0);
                cmd.Parameters.AddWithValue("$isTor", record.IsTor ? 1 : 0);
                cmd.Parameters.AddWithValue("$riskScore", record.RiskScore);
                cmd.Parameters.AddWithValue("$provider", record.Provider ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$countryCode", record.CountryCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$city", record.City ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$region", record.Region ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$cacheExpiry", record.CacheExpiry.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$lastChecked", record.LastChecked.ToString("yyyy-MM-dd HH:mm:ss"));

                int rowsAffected = cmd.ExecuteNonQuery();
                
                tx.Commit();

                AppDebug.Log($"Updated proxy record ID: {record.RecordID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to update proxy record ID: {record.RecordID}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Remove a proxy record from the database by RecordID.
        /// </summary>
        public static bool RemoveProxyRecord(int recordId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_proxyRecords
                    WHERE RecordID = $recordId;
                ";
                cmd.Parameters.AddWithValue("$recordId", recordId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Removed proxy record ID: {recordId}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to remove proxy record ID: {recordId}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Find a proxy record by IP address.
        /// </summary>
        public static proxyRecord? GetProxyRecordByIP(IPAddress ipAddress)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT RecordID, IPAddress, IsVpn, IsProxy, IsTor, RiskScore, 
                       Provider, CountryCode, City, Region, CacheExpiry, LastChecked
                FROM tb_proxyRecords
                WHERE IPAddress = $ipAddress
                LIMIT 1;
            ";
            cmd.Parameters.AddWithValue("$ipAddress", ipAddress.ToString());

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new proxyRecord
                {
                    RecordID = reader.GetInt32(0),
                    IPAddress = IPAddress.Parse(reader.GetString(1)),
                    IsVpn = reader.GetInt32(2) == 1,
                    IsProxy = reader.GetInt32(3) == 1,
                    IsTor = reader.GetInt32(4) == 1,
                    RiskScore = reader.GetInt32(5),
                    Provider = reader.IsDBNull(6) ? null : reader.GetString(6),
                    CountryCode = reader.IsDBNull(7) ? null : reader.GetString(7),
                    City = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Region = reader.IsDBNull(9) ? null : reader.GetString(9),
                    CacheExpiry = DateTime.Parse(reader.GetString(10)),
                    LastChecked = DateTime.Parse(reader.GetString(11))
                };
            }

            return null;
        }

        // ================================================================================
        // USER MANAGEMENT METHODS
        // ================================================================================

        /// <summary>
        /// Get all users from the database.
        /// </summary>
        public static List<UserRecord> GetAllUsers()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var users = new List<UserRecord>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT UserID, Username, IsActive, Created, LastLogin, Notes
                FROM tb_users
                ORDER BY Username COLLATE NOCASE;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new UserRecord
                {
                    UserID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    IsActive = reader.GetInt32(2) == 1,
                    Created = reader.IsDBNull(3) ? DateTime.MinValue : DateTime.SpecifyKind(DateTime.Parse(reader.GetString(3)), DateTimeKind.Utc),
                    LastLogin = reader.IsDBNull(4) ? null : DateTime.SpecifyKind(DateTime.Parse(reader.GetString(4)), DateTimeKind.Utc),
                    Notes = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                });
            }

            AppDebug.Log($"Loaded {users.Count} users", AppDebug.LogLevel.Info);
            return users;
        }

        /// <summary>
        /// Get a single user by username.
        /// </summary>
        public static UserRecord? GetUserByUsername(string username)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT UserID, Username, PasswordHash, Salt, IsActive, Created, LastLogin, Notes
                FROM tb_users
                WHERE Username = $username COLLATE NOCASE
                LIMIT 1;
            ";
            cmd.Parameters.AddWithValue("$username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserRecord
                {
                    UserID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    Salt = reader.GetString(3),
                    IsActive = reader.GetInt32(4) == 1,
                    Created = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.SpecifyKind(DateTime.Parse(reader.GetString(5)), DateTimeKind.Utc),
                    LastLogin = reader.IsDBNull(6) ? null : DateTime.SpecifyKind(DateTime.Parse(reader.GetString(6)), DateTimeKind.Utc),
                    Notes = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                };
            }

            return null;
        }

        /// <summary>
        /// Get a single user by UserID.
        /// </summary>
        public static UserRecord? GetUserByID(int userID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT UserID, Username, PasswordHash, Salt, IsActive, Created, LastLogin, Notes
                FROM tb_users
                WHERE UserID = $userID
                LIMIT 1;
            ";
            cmd.Parameters.AddWithValue("$userID", userID);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserRecord
                {
                    UserID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    Salt = reader.GetString(3),
                    IsActive = reader.GetInt32(4) == 1,
                    Created = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.SpecifyKind(DateTime.Parse(reader.GetString(5)), DateTimeKind.Utc),
                    LastLogin = reader.IsDBNull(6) ? null : DateTime.SpecifyKind(DateTime.Parse(reader.GetString(6)), DateTimeKind.Utc),
                    Notes = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                };
            }

            return null;
        }

        /// <summary>
        /// Add a new user to the database.
        /// </summary>
        public static int AddUser(string username, string passwordHash, string salt, bool isActive, string notes)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_users (Username, PasswordHash, Salt, IsActive, Notes, Created)
                    VALUES ($username, $passwordHash, $salt, $isActive, $notes, datetime('now'));
                    SELECT last_insert_rowid();
                ";

                cmd.Parameters.AddWithValue("$username", username);
                cmd.Parameters.AddWithValue("$passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("$salt", salt);
                cmd.Parameters.AddWithValue("$isActive", isActive ? 1 : 0);
                cmd.Parameters.AddWithValue("$notes", notes ?? string.Empty);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();

                AppDebug.Log($"Added user: {username} (ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to add user: {username}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Update an existing user (without changing password).
        /// </summary>
        public static bool UpdateUser(int userID, string username, bool isActive, string notes)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_users 
                    SET Username = $username,
                        IsActive = $isActive,
                        Notes = $notes
                    WHERE UserID = $userID;
                ";

                cmd.Parameters.AddWithValue("$userID", userID);
                cmd.Parameters.AddWithValue("$username", username);
                cmd.Parameters.AddWithValue("$isActive", isActive ? 1 : 0);
                cmd.Parameters.AddWithValue("$notes", notes ?? string.Empty);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Updated user ID: {userID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to update user ID: {userID}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Update a user's password.
        /// </summary>
        public static bool UpdateUserPassword(int userID, string passwordHash, string salt)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_users 
                    SET PasswordHash = $passwordHash,
                        Salt = $salt
                    WHERE UserID = $userID;
                ";

                cmd.Parameters.AddWithValue("$userID", userID);
                cmd.Parameters.AddWithValue("$passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("$salt", salt);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Updated password for user ID: {userID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to update password for user ID: {userID}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Delete a user from the database.
        /// </summary>
        public static bool DeleteUser(int userID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_users
                    WHERE UserID = $userID;
                ";
                cmd.Parameters.AddWithValue("$userID", userID);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Deleted user ID: {userID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Update a user's last login timestamp.
        /// </summary>
        public static bool UpdateUserLastLogin(int userID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_users 
                    SET LastLogin = datetime('now')
                    WHERE UserID = $userID;
                ";
                cmd.Parameters.AddWithValue("$userID", userID);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Get all permissions for a user.
        /// </summary>
        public static List<string> GetUserPermissions(int userID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var permissions = new List<string>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Permission
                FROM tb_userPermissions
                WHERE UserID = $userID
                ORDER BY Permission;
            ";
            cmd.Parameters.AddWithValue("$userID", userID);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                permissions.Add(reader.GetString(0));
            }

            return permissions;
        }

        /// <summary>
        /// Add a permission to a user.
        /// </summary>
        public static bool AddUserPermission(int userID, string permission)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            if (string.IsNullOrWhiteSpace(permission))
                throw new ArgumentException("Permission cannot be empty.", nameof(permission));

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_userPermissions (UserID, Permission)
                    VALUES ($userID, $permission);
                ";
                cmd.Parameters.AddWithValue("$userID", userID);
                cmd.Parameters.AddWithValue("$permission", permission.ToLower());

                cmd.ExecuteNonQuery();
                tx.Commit();

                return true;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint
            {
                tx.Rollback();
                // Permission already exists for this user - not an error
                return false;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Delete all permissions for a user.
        /// </summary>
        public static bool DeleteAllUserPermissions(int userID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_userPermissions
                    WHERE UserID = $userID;
                ";
                cmd.Parameters.AddWithValue("$userID", userID);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Deleted {rowsAffected} permissions for user ID: {userID}", AppDebug.LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to delete permissions for user ID: {userID}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Check if a username already exists (case-insensitive).
        /// </summary>
        public static bool UsernameExists(string username)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(*) 
                FROM tb_users 
                WHERE Username = $username COLLATE NOCASE;
            ";
            cmd.Parameters.AddWithValue("$username", username);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Check if a username exists for a different user (for updates).
        /// </summary>
        public static bool UsernameExistsForOtherUser(string username, int excludeUserID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(*) 
                FROM tb_users 
                WHERE Username = $username COLLATE NOCASE 
                AND UserID != $excludeUserID;
            ";
            cmd.Parameters.AddWithValue("$username", username);
            cmd.Parameters.AddWithValue("$excludeUserID", excludeUserID);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Releases the exclusive lock and closes the internal connection.
        /// Call this at application shutdown.
        /// </summary>

        /// <summary>
        /// Log an audit action to the database
        /// </summary>
        public static void LogAuditAction(
            int? userId,
            string username,
            string category,
            string actionType,
            string description,
            string? targetType = null,
            string? targetId = null,
            string? targetName = null,
            string? oldValue = null,
            string? newValue = null,
            string? ipAddress = null,
            bool success = true,
            string? errorMessage = null,
            string? metadata = null)
        {
            try
            {
                AppDebug.Log($"LogAuditAction called: User={username}, Category={category}, Action={actionType}, Success={success}", AppDebug.LogLevel.Info);
                
                if (!IsInitialized)
                {
                    AppDebug.Log("Cannot log audit action: DatabaseManager is not initialized.", AppDebug.LogLevel.Info);
                    return;
                }

                using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
                conn.Open();

                using var tx = conn.BeginTransaction();
                using var cmd = conn.CreateCommand();

                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_auditLogs 
                    (Timestamp, UserID, Username, ActionCategory, ActionType, ActionDescription, 
                     TargetType, TargetID, TargetName, OldValue, NewValue, IPAddress, 
                     Success, ErrorMessage, Metadata)
                    VALUES 
                    ($Timestamp, $UserID, $Username, $ActionCategory, $ActionType, $ActionDescription,
                     $TargetType, $TargetID, $TargetName, $OldValue, $NewValue, $IPAddress,
                     $Success, $ErrorMessage, $Metadata)";

                cmd.Parameters.AddWithValue("$Timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("$UserID", userId.HasValue ? userId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("$Username", username);
                cmd.Parameters.AddWithValue("$ActionCategory", category);
                cmd.Parameters.AddWithValue("$ActionType", actionType);
                cmd.Parameters.AddWithValue("$ActionDescription", description);
                cmd.Parameters.AddWithValue("$TargetType", targetType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$TargetID", targetId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$TargetName", targetName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$OldValue", oldValue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$NewValue", newValue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$IPAddress", ipAddress ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$Success", success ? 1 : 0);
                cmd.Parameters.AddWithValue("$ErrorMessage", errorMessage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$Metadata", metadata ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                tx.Commit();
                
                AppDebug.Log($"Audit log successfully written for {username}", AppDebug.LogLevel.Info);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Failed to log audit action", AppDebug.LogLevel.Error, ex);
            }
        }

        /// <summary>
        /// Get audit logs with filtering and pagination
        /// </summary>
        public static (List<AuditLogDTO> Logs, int TotalCount) GetAuditLogs(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? usernameFilter = null,
            string? categoryFilter = null,
            string? actionTypeFilter = null,
            string? targetFilter = null,
            bool? successOnly = null,
            int limit = 1000,
            int offset = 0)
        {
            var logs = new List<AuditLogDTO>();
            int totalCount = 0;

            try
            {
                AppDebug.Log($"GetAuditLogs called: Start={startDate}, End={endDate}, User={usernameFilter}, Category={categoryFilter}", AppDebug.LogLevel.Info);
                
                if (!IsInitialized)
                {
                    AppDebug.Log("GetAuditLogs: DatabaseManager is not initialized.", AppDebug.LogLevel.Info);
                    throw new InvalidOperationException("DatabaseManager is not initialized.");
                }

                using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
                conn.Open();

                var whereClauses = new List<string>();

                if (startDate.HasValue)
                    whereClauses.Add("datetime(Timestamp) >= $StartDate");
                if (endDate.HasValue)
                    whereClauses.Add("datetime(Timestamp) <= $EndDate");
                if (!string.IsNullOrEmpty(usernameFilter))
                    whereClauses.Add("Username LIKE $Username");
                if (!string.IsNullOrEmpty(categoryFilter))
                    whereClauses.Add("ActionCategory = $Category");
                if (!string.IsNullOrEmpty(actionTypeFilter))
                    whereClauses.Add("ActionType = $ActionType");
                if (!string.IsNullOrEmpty(targetFilter))
                    whereClauses.Add("(TargetName LIKE $Target OR TargetID LIKE $Target)");
                if (successOnly.HasValue)
                    whereClauses.Add(successOnly.Value ? "Success = 1" : "Success = 0");

                string whereClause = whereClauses.Count > 0
                    ? "WHERE " + string.Join(" AND ", whereClauses)
                    : "";

                AppDebug.Log($"GetAuditLogs: Query WHERE clause: {whereClause}", AppDebug.LogLevel.Info);

                // Get total count
                string countSql = $"SELECT COUNT(*) FROM tb_auditLogs {whereClause}";
                using (var countCmd = conn.CreateCommand())
                {
                    countCmd.CommandText = countSql;
                    AddAuditFilterParameters(countCmd, startDate, endDate, usernameFilter,
                        categoryFilter, actionTypeFilter, targetFilter);
                    totalCount = Convert.ToInt32(countCmd.ExecuteScalar() ?? 0);
                    AppDebug.Log($"GetAuditLogs: Total count = {totalCount}", AppDebug.LogLevel.Info);
                }

                // Get logs
                string sql = $@"
                    SELECT LogID, Timestamp, UserID, Username, ActionCategory, ActionType,
                           ActionDescription, TargetType, TargetID, TargetName, OldValue,
                           NewValue, IPAddress, Success, ErrorMessage, Metadata
                    FROM tb_auditLogs
                    {whereClause}
                    ORDER BY Timestamp DESC
                    LIMIT $Limit OFFSET $Offset";

                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                AddAuditFilterParameters(cmd, startDate, endDate, usernameFilter,
                    categoryFilter, actionTypeFilter, targetFilter);
                cmd.Parameters.AddWithValue("$Limit", limit);
                cmd.Parameters.AddWithValue("$Offset", offset);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    logs.Add(new AuditLogDTO
                    {
                        LogID = reader.GetInt32(0),
                        Timestamp = DateTime.SpecifyKind(DateTime.Parse(reader.GetString(1)), DateTimeKind.Utc),
                        UserID = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                        Username = reader.GetString(3),
                        ActionCategory = reader.GetString(4),
                        ActionType = reader.GetString(5),
                        ActionDescription = reader.GetString(6),
                        TargetType = reader.IsDBNull(7) ? null : reader.GetString(7),
                        TargetID = reader.IsDBNull(8) ? null : reader.GetString(8),
                        TargetName = reader.IsDBNull(9) ? null : reader.GetString(9),
                        OldValue = reader.IsDBNull(10) ? null : reader.GetString(10),
                        NewValue = reader.IsDBNull(11) ? null : reader.GetString(11),
                        IPAddress = reader.IsDBNull(12) ? null : reader.GetString(12),
                        Success = reader.GetInt32(13) == 1,
                        ErrorMessage = reader.IsDBNull(14) ? null : reader.GetString(14),
                        Metadata = reader.IsDBNull(15) ? null : reader.GetString(15)
                    });
                }

                AppDebug.Log($"Retrieved {logs.Count} of {totalCount} audit logs", AppDebug.LogLevel.Info);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Failed to retrieve audit logs", AppDebug.LogLevel.Error, ex);
            }

            return (logs, totalCount);
        }

        /// <summary>
        /// Helper method to add filter parameters to audit log queries
        /// </summary>
        private static void AddAuditFilterParameters(SqliteCommand cmd,
            DateTime? startDate, DateTime? endDate, string? username,
            string? category, string? actionType, string? target)
        {
            if (startDate.HasValue)
                cmd.Parameters.AddWithValue("$StartDate", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (endDate.HasValue)
                cmd.Parameters.AddWithValue("$EndDate", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(username))
                cmd.Parameters.AddWithValue("$Username", $"%{username}%");
            if (!string.IsNullOrEmpty(category))
                cmd.Parameters.AddWithValue("$Category", category);
            if (!string.IsNullOrEmpty(actionType))
                cmd.Parameters.AddWithValue("$ActionType", actionType);
            if (!string.IsNullOrEmpty(target))
                cmd.Parameters.AddWithValue("$Target", $"%{target}%");
        }

        /// <summary>
        /// Delete audit logs older than specified days
        /// </summary>
        public static int DeleteOldAuditLogs(int daysToKeep = 90)
        {
            try
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("DatabaseManager is not initialized.");

                using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
                conn.Open();

                using var tx = conn.BeginTransaction();
                using var cmd = conn.CreateCommand();

                cmd.Transaction = tx;
                cmd.CommandText = @"DELETE FROM tb_auditLogs 
                                   WHERE datetime(Timestamp) < datetime('now', $Days)";

                cmd.Parameters.AddWithValue("$Days", $"-{daysToKeep} days");

                int deletedCount = cmd.ExecuteNonQuery();
                tx.Commit();

                AppDebug.Log($"Deleted {deletedCount} old audit log records (kept {daysToKeep} days)", AppDebug.LogLevel.Info);
                return deletedCount;
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Failed to delete old audit logs", AppDebug.LogLevel.Error, ex);
                return 0;
            }
        }

        /// <summary>
        /// Get distinct categories from audit logs (for filter dropdown)
        /// </summary>
        public static List<string> GetAuditCategories()
        {
            var categories = new List<string>();

            try
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("DatabaseManager is not initialized.");

                using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT ActionCategory FROM tb_auditLogs ORDER BY ActionCategory";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Failed to get audit categories", AppDebug.LogLevel.Error, ex);
            }

            return categories;
        }

        /// <summary>
        /// Get distinct action types from audit logs (for filter dropdown)
        /// </summary>
        public static List<string> GetAuditActionTypes()
        {
            var actionTypes = new List<string>();

            try
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("DatabaseManager is not initialized.");

                using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT ActionType FROM tb_auditLogs ORDER BY ActionType";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    actionTypes.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Failed to get audit action types", AppDebug.LogLevel.Error, ex);
            }

            return actionTypes;
        }

        public static void Shutdown()
        {
            if (_connection == null)
            {
                return;
            }

            try
            {
                // No need to commit writes as they are made immediately
                _connection.Close();
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                // Handle any exceptions during shutdown gracefully
                AppDebug.Log("Error during database shutdown", AppDebug.LogLevel.Error, ex);
            }
            finally
            {
                _connection = null;
            }
        }

        /// <summary>
        /// Get all Babstats servers from the database.
        /// </summary>
        public static List<BabstatsServerSettings> GetBabstatsServers()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var servers = new List<BabstatsServerSettings>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT BabstatsServerID, DisplayName, ServerPath, ProfileID,
                       IsEnabled, EnableAnnouncements, ReportIntervalSeconds,
                       UpdateIntervalSeconds, SortOrder
                FROM tb_babstatsServers
                ORDER BY SortOrder, BabstatsServerID;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                servers.Add(new BabstatsServerSettings(
                    BabstatsServerID: reader.GetInt32(0),
                    DisplayName: reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    ServerPath: reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    ProfileID: reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    IsEnabled: reader.GetInt32(4) == 1,
                    EnableAnnouncements: reader.GetInt32(5) == 1,
                    ReportIntervalSeconds: reader.GetInt32(6),
                    UpdateIntervalSeconds: reader.GetInt32(7),
                    SortOrder: reader.GetInt32(8)
                ));
            }

            return servers;
        }

        /// <summary>
        /// Add a new Babstats server to the database.
        /// </summary>
        public static int AddBabstatsServer(BabstatsServerSettings server)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_babstatsServers
                    (DisplayName, ServerPath, ProfileID, IsEnabled, EnableAnnouncements, ReportIntervalSeconds, UpdateIntervalSeconds, SortOrder, CreatedUtc, UpdatedUtc)
                    VALUES
                    ($displayName, $serverPath, $profileId, $isEnabled, $announcements, $reportInterval, $updateInterval, $sortOrder, datetime('now'), datetime('now'));
                    SELECT last_insert_rowid();
                ";
                cmd.Parameters.AddWithValue("$displayName", server.DisplayName ?? string.Empty);
                cmd.Parameters.AddWithValue("$serverPath", server.ServerPath ?? string.Empty);
                cmd.Parameters.AddWithValue("$profileId", server.ProfileID ?? string.Empty);
                cmd.Parameters.AddWithValue("$isEnabled", server.IsEnabled ? 1 : 0);
                cmd.Parameters.AddWithValue("$announcements", server.EnableAnnouncements ? 1 : 0);
                cmd.Parameters.AddWithValue("$reportInterval", server.ReportIntervalSeconds);
                cmd.Parameters.AddWithValue("$updateInterval", server.UpdateIntervalSeconds);
                cmd.Parameters.AddWithValue("$sortOrder", server.SortOrder);

                var newId = (long)cmd.ExecuteScalar()!;
                tx.Commit();
                AppDebug.Log($"Added Babstats server: {server.DisplayName} (ID: {newId})", AppDebug.LogLevel.Info);
                return (int)newId;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to add Babstats server: {server.DisplayName}", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Update an existing Babstats server in the database.
        /// </summary>
        public static bool UpdateBabstatsServer(BabstatsServerSettings server)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_babstatsServers
                    SET DisplayName = $displayName,
                        ServerPath = $serverPath,
                        ProfileID = $profileId,
                        IsEnabled = $isEnabled,
                        EnableAnnouncements = $announcements,
                        ReportIntervalSeconds = $reportInterval,
                        UpdateIntervalSeconds = $updateInterval,
                        SortOrder = $sortOrder,
                        UpdatedUtc = datetime('now')
                    WHERE BabstatsServerID = $id;
                ";
                cmd.Parameters.AddWithValue("$id", server.BabstatsServerID);
                cmd.Parameters.AddWithValue("$displayName", server.DisplayName ?? string.Empty);
                cmd.Parameters.AddWithValue("$serverPath", server.ServerPath ?? string.Empty);
                cmd.Parameters.AddWithValue("$profileId", server.ProfileID ?? string.Empty);
                cmd.Parameters.AddWithValue("$isEnabled", server.IsEnabled ? 1 : 0);
                cmd.Parameters.AddWithValue("$announcements", server.EnableAnnouncements ? 1 : 0);
                cmd.Parameters.AddWithValue("$reportInterval", server.ReportIntervalSeconds);
                cmd.Parameters.AddWithValue("$updateInterval", server.UpdateIntervalSeconds);
                cmd.Parameters.AddWithValue("$sortOrder", server.SortOrder);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();
                AppDebug.Log($"Updated Babstats server ID: {server.BabstatsServerID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                return false;
            }
        }

        /// <summary>
        /// Remove a Babstats server from the database by ID.
        /// </summary>
        public static bool RemoveBabstatsServer(int babstatsServerId)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_babstatsServers
                    WHERE BabstatsServerID = $id;
                ";
                cmd.Parameters.AddWithValue("$id", babstatsServerId);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();
                AppDebug.Log($"Removed Babstats server ID: {babstatsServerId}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to remove Babstats server ID: {babstatsServerId}", AppDebug.LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Set EnableAnnouncements to false for all Babstats servers.
        /// </summary>
        public static void DisableAllBabstatsAnnouncements()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_babstatsServers
                    SET EnableAnnouncements = 0,
                        UpdatedUtc = datetime('now');
                ";
                cmd.ExecuteNonQuery();
                tx.Commit();
                AppDebug.Log("Set EnableAnnouncements = 0 for all Babstats servers", AppDebug.LogLevel.Info);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log("Failed to disable Babstats announcements", AppDebug.LogLevel.Error, ex);
                throw;
            }
        }

        public static List<LobbyServerSettings> GetLobbyServers()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var servers = new List<LobbyServerSettings>();

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT LobbyServerID, SiteName, ServerUri, GamePort, SecretKey, IsEnabled, SortOrder
                FROM tb_lobbyServers
                ORDER BY SortOrder, LobbyServerID;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                servers.Add(new LobbyServerSettings(
                    LobbyServerID: reader.GetInt32(0),
                    SiteName: reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    ServerUri: reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    GamePort: reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    SecretKey: reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    IsEnabled: reader.GetInt32(5) == 1,
                    SortOrder: reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                ));
            }

            return servers;
        }

        public static bool UpdateLobbyServer(LobbyServerSettings server)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    UPDATE tb_lobbyServers
                    SET SiteName = $siteName,
                        ServerUri = $serverUri,
                        GamePort = $gamePort,
                        SecretKey = $secretKey,
                        IsEnabled = $isEnabled,
                        SortOrder = $sortOrder,
                        UpdatedUtc = datetime('now')
                    WHERE LobbyServerID = $id;
                ";
                cmd.Parameters.AddWithValue("$id", server.LobbyServerID);
                cmd.Parameters.AddWithValue("$siteName", server.SiteName ?? string.Empty);
                cmd.Parameters.AddWithValue("$serverUri", server.ServerUri ?? string.Empty);
                cmd.Parameters.AddWithValue("$gamePort", server.GamePort);
                cmd.Parameters.AddWithValue("$secretKey", server.SecretKey ?? string.Empty);
                cmd.Parameters.AddWithValue("$isEnabled", server.IsEnabled ? 1 : 0);
                cmd.Parameters.AddWithValue("$sortOrder", server.SortOrder);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();
                AppDebug.Log($"Updated lobby server ID: {server.LobbyServerID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                return false;
            }
        }

        public static bool AddLobbyServer(LobbyServerSettings server)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    INSERT INTO tb_lobbyServers
                        (SiteName, ServerUri, GamePort, SecretKey, IsEnabled, SortOrder, CreatedUtc, UpdatedUtc)
                    VALUES
                        ($siteName, $serverUri, $gamePort, $secretKey, $isEnabled, $sortOrder, datetime('now'), datetime('now'));
                ";
                cmd.Parameters.AddWithValue("$siteName", server.SiteName ?? string.Empty);
                cmd.Parameters.AddWithValue("$serverUri", server.ServerUri ?? string.Empty);
                cmd.Parameters.AddWithValue("$gamePort", server.GamePort);
                cmd.Parameters.AddWithValue("$secretKey", server.SecretKey ?? string.Empty);
                cmd.Parameters.AddWithValue("$isEnabled", server.IsEnabled ? 1 : 0);
                cmd.Parameters.AddWithValue("$sortOrder", server.SortOrder);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();
                AppDebug.Log($"Added lobby server: {server.SiteName} ({server.ServerUri})", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
                return false;
            }
        }

        public static bool RemoveLobbyServer(int lobbyID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            using var conn = new SqliteConnection($"Data Source={_databasePath};Mode=ReadWrite;");
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = @"
                    DELETE FROM tb_lobbyServers
                    WHERE LobbyServerID = $id;
                ";
                cmd.Parameters.AddWithValue("$id", lobbyID);

                int rowsAffected = cmd.ExecuteNonQuery();
                tx.Commit();
                AppDebug.Log($"Removed lobby server ID: {lobbyID}", AppDebug.LogLevel.Info);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log($"Failed to remove lobby server ID: {lobbyID}", AppDebug.LogLevel.Error, ex);
                return false;
            }
        }


	}
}