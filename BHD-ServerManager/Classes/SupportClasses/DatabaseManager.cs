using HawkSyncShared.DTOs;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.tabMaps;
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

        public static bool IsInitialized => _connection != null;

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            string databasePath = _databasePath;

            // Check if the database file exists
            if (!File.Exists(databasePath))
            {
                AppDebug.Log("DatabaseManager", "Failed to locate the database file: " + databasePath);
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
                    AppDebug.Log("DatabaseManager", $"DatabaseManager: GetSetting: ({key} - {result}) Default:({defaultValue})");
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
            AppDebug.Log("DatabaseManager", $"DatabaseManager: SetSetting: ({key} - {value})");
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

            AppDebug.Log("DatabaseManager", $"DatabaseManager: Loaded {maps.Count} default maps");

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

            AppDebug.Log("DatabaseManager", $"DatabaseManager: Loaded {maps.Count} maps for playlist {playlistID}");

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
                AppDebug.Log("DatabaseManager", $"DatabaseManager: Saved playlist {playlistID} ({maps.Count} maps)");
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

            AppDebug.Log("DatabaseManager", $"Loaded {messages.Count} slap messages");
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

                AppDebug.Log("DatabaseManager", $"Added slap message: {messageText} (ID: {newId})");
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

                AppDebug.Log("DatabaseManager", $"Removed slap message ID: {slapMessageId}");
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

            AppDebug.Log("DatabaseManager", $"Loaded {messages.Count} auto messages");
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

                AppDebug.Log("DatabaseManager", $"Added auto message: {messageText} (Trigger: {triggerSeconds}s, ID: {newId})");
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

                AppDebug.Log("DatabaseManager", $"Removed auto message ID: {autoMessageId}");
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

            AppDebug.Log("DatabaseManager", $"Loaded {logs.Count} chat log entries");
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
                        AppDebug.Log("DatabaseManager", $"Warning: Invalid Unix timestamp {unixTimestamp}, attempting TEXT parse");
                    }
                }
            }
            catch (InvalidCastException)
            {
                // Column is TEXT, fall through to string parsing
            }

            try
            {
                // Fallback: Try to read as TEXT (legacy format)
                string textTimestamp = reader.GetString(columnIndex);
                
                if (DateTime.TryParse(textTimestamp, out DateTime parsedDate))
                {
                    AppDebug.Log("DatabaseManager", $"Warning: Found TEXT timestamp (legacy format): {textTimestamp}");
                    return parsedDate;
                }
                else
                {
                    AppDebug.Log("DatabaseManager", $"Error: Failed to parse timestamp: {textTimestamp}");
                    return DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("DatabaseManager", $"Error reading timestamp: {ex.Message}");
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

            AppDebug.Log("DatabaseManager", $"Loaded {players.Count} distinct player names");
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

            AppDebug.Log("DatabaseManager", $"Loaded {logs.Count} of {totalCount} chat logs (Page {page})");
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
                                AppDebug.Log("DatabaseManager", "Chat logs timestamps already migrated to INTEGER");
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

                AppDebug.Log("DatabaseManager", $"Starting migration of {originalCount} chat log records...");

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
                    
                    AppDebug.Log("DatabaseManager", $"Successfully migrated {rowsAffected} records");
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
                            throw new Exception($"Invalid timestamp after migration: {timestamp}");
                        }
                        AppDebug.Log("DatabaseManager", $"Validation passed. Sample timestamp: {timestamp}");
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
                AppDebug.Log("DatabaseManager", "✅ Chat logs timestamp migration completed successfully");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                AppDebug.Log("DatabaseManager", $"❌ Migration failed: {ex.Message}");
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

            AppDebug.Log("DatabaseManager", $"Loaded {records.Count} proxy records");
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

            AppDebug.Log("DatabaseManager", $"Loaded {records.Count} player name records for category {category}");
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

            AppDebug.Log("DatabaseManager", $"Loaded {records.Count} player IP records for category {category}");
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

                AppDebug.Log("DatabaseManager", $"Added player name record: {record.PlayerName} (ID: {newId})");
                return (int)newId;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Added player IP record: {record.PlayerIP}/{record.SubnetMask} (ID: {newId})");
                return (int)newId;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Updated player name record ID: {record.RecordID}");
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

                AppDebug.Log("DatabaseManager", $"Updated player IP record ID: {record.RecordID}");
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

                AppDebug.Log("DatabaseManager", $"Removed player name record ID: {recordId}");
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

                AppDebug.Log("DatabaseManager", $"Removed player IP record ID: {recordId}");
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

            AppDebug.Log("DatabaseManager", $"Loaded {countries.Count} blocked countries");
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

                AppDebug.Log("DatabaseManager", $"Added blocked country: {countryCode} - {countryName} (ID: {newId})");
                return (int)newId;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint
            {
                tx.Rollback();
                AppDebug.Log("DatabaseManager", $"Country code {countryCode} already exists");
                return -1;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Removed blocked country ID: {recordId}");
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

                AppDebug.Log("DatabaseManager", $"Added proxy record: {record.IPAddress} (ID: {newId})");
                return (int)newId;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint
            {
                tx.Rollback();
                AppDebug.Log("DatabaseManager", $"Proxy record for {record.IPAddress} already exists");
                return -1;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Updated proxy record ID: {record.RecordID}");
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Removed proxy record ID: {recordId}");
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
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
                    Created = reader.IsDBNull(3) ? DateTime.MinValue : DateTime.Parse(reader.GetString(3)),
                    LastLogin = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
                    Notes = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                });
            }

            AppDebug.Log("DatabaseManager", $"Loaded {users.Count} users");
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
                    Created = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.Parse(reader.GetString(5)),
                    LastLogin = reader.IsDBNull(6) ? null : DateTime.Parse(reader.GetString(6)),
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
                    Created = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.Parse(reader.GetString(5)),
                    LastLogin = reader.IsDBNull(6) ? null : DateTime.Parse(reader.GetString(6)),
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

                AppDebug.Log("DatabaseManager", $"Added user: {username} (ID: {newId})");
                return (int)newId;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Updated user ID: {userID}");
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Updated password for user ID: {userID}");
                return rowsAffected > 0;
            }
            catch
            {
                tx.Rollback();
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

                AppDebug.Log("DatabaseManager", $"Deleted user ID: {userID}");
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

                AppDebug.Log("DatabaseManager", $"Deleted {rowsAffected} permissions for user ID: {userID}");
                return true;
            }
            catch
            {
                tx.Rollback();
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
                AppDebug.Log("DatabaseManager", "Error during database shutdown: " + ex.Message);
            }
            finally
            {
                _connection = null;
            }
        }
    }
}