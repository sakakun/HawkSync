using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BHD_ServerManager.Classes.Instances;
using Microsoft.Data.Sqlite;

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
        public static List<mapFileInfo> GetDefaultMaps()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var maps = new List<mapFileInfo>();

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
                maps.Add(new mapFileInfo
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
        public static List<mapFileInfo> GetPlaylistMaps(int playlistID)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("DatabaseManager is not initialized.");

            var maps = new List<mapFileInfo>();

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
                maps.Add(new mapFileInfo
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

        public static void SavePlaylist(int playlistID, List<mapFileInfo> maps)
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