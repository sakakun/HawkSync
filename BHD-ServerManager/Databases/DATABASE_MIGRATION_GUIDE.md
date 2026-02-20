# Database Migration Guide

## Overview

The database migration system automatically upgrades the SQLite database schema from the **`database.sqlite.sql`** file. This file is the single source of truth for your database schema, making upgrades simple and maintainable.

## How It Works

1. **Version Tracking**: The current schema version is stored in `tb_settings` with key `schema_version`
2. **Automatic Backup**: Before any migration, a timestamped backup is created (e.g., `database.sqlite.backup_20240115_143022`)
3. **SQL File Parsing**: The system reads `database.sqlite.sql` and extracts CREATE TABLE and CREATE INDEX statements
4. **Smart Application**: 
   - New tables are created automatically
   - Existing tables are checked for missing columns and updated
   - Indexes are created if they don't exist
5. **Rollback on Failure**: If any migration fails, all changes are rolled back

## Adding a Schema Change (Simple Workflow)

### Step 1: Update database.sqlite.sql

Edit the `database.sqlite.sql` file with your changes:

```sql
-- Add a new table
CREATE TABLE IF NOT EXISTS tb_newFeature (
    FeatureID INTEGER PRIMARY KEY AUTOINCREMENT,
    FeatureName TEXT NOT NULL,
    IsEnabled INTEGER DEFAULT 1
);

-- Add a column to existing table (just add it to the CREATE TABLE statement)
CREATE TABLE IF NOT EXISTS tb_users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE COLLATE NOCASE,
    PasswordHash TEXT NOT NULL,
    Salt TEXT NOT NULL,
    IsActive INTEGER DEFAULT 1,
    Created TEXT DEFAULT (datetime('now')),
    LastLogin TEXT,
    Notes TEXT DEFAULT '',
    Email TEXT DEFAULT ''  -- NEW COLUMN
);

-- Add indexes
CREATE INDEX IF NOT EXISTS idx_newFeature_name ON tb_newFeature(FeatureName);
```

### Step 2: Increment Schema Version

In `DatabaseManager.cs`, increment the version:

```csharp
private const int CURRENT_SCHEMA_VERSION = 2; // Changed from 1 to 2
```

### Step 3: Done!

That's it! On next application startup:
- The system detects the version mismatch
- Creates a backup
- Parses `database.sqlite.sql`
- Creates new tables and indexes
- Adds missing columns to existing tables
- Updates the schema version

## What Gets Applied Automatically

? **New Tables** - Created with full structure
? **New Columns** - Added to existing tables with DEFAULT values
? **New Indexes** - Created automatically
? **No Data Loss** - All existing data is preserved

## Complex Migrations (Manual)

Some changes cannot be handled automatically and require custom migration code:

- ?? Renaming columns
- ?? Changing column types
- ?? Removing columns
- ?? Complex data transformations
- ?? Restructuring relationships

For these cases, add custom migration logic to `ApplyCustomMigrations()` in `DatabaseManager.cs`:

```csharp
private static void ApplyCustomMigrations(SqliteConnection conn, SqliteTransaction tx, int fromVersion, int toVersion)
{
    for (int version = fromVersion + 1; version <= toVersion; version++)
    {
        switch (version)
        {
            case 2:
                MigrateCustomVersion2(conn, tx);
                break;
        }
    }
}

private static void MigrateCustomVersion2(SqliteConnection conn, SqliteTransaction tx)
{
    using var cmd = conn.CreateCommand();
    cmd.Transaction = tx;

    // Example: Rename a column (SQLite doesn't support direct RENAME COLUMN)
    // 1. Create new table with correct structure
    cmd.CommandText = @"
        CREATE TABLE tb_users_new (
            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL,
            EmailAddress TEXT  -- Renamed from Email
        );";
    cmd.ExecuteNonQuery();

    // 2. Copy data
    cmd.CommandText = @"
        INSERT INTO tb_users_new (UserID, Username, EmailAddress)
        SELECT UserID, Username, Email FROM tb_users;";
    cmd.ExecuteNonQuery();

    // 3. Drop old table
    cmd.CommandText = "DROP TABLE tb_users;";
    cmd.ExecuteNonQuery();

    // 4. Rename new table
    cmd.CommandText = "ALTER TABLE tb_users_new RENAME TO tb_users;";
    cmd.ExecuteNonQuery();

    // 5. Recreate indexes (from SQL file)
    cmd.CommandText = @"
        CREATE INDEX IF NOT EXISTS idx_users_username ON tb_users(Username);
        CREATE INDEX IF NOT EXISTS idx_users_active ON tb_users(IsActive);";
    cmd.ExecuteNonQuery();

    AppDebug.Log("DatabaseManager", "Applied custom migrations for version 2");
}
```

## Backup System

- Backups are created automatically before migrations
- Format: `database.sqlite.backup_YYYYMMDD_HHmmss`
- Only the 5 most recent backups are kept
- Location: Same directory as the database file (`Databases/`)

## Workflow Example

Let's say you want to add a notifications feature:

### 1. Update database.sqlite.sql

Add to your SQL file:

```sql
CREATE TABLE IF NOT EXISTS tb_notifications (
    NotificationID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    Title TEXT NOT NULL,
    Message TEXT NOT NULL,
    IsRead INTEGER NOT NULL DEFAULT 0,
    CreatedDate TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY(UserID) REFERENCES tb_users(UserID) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_notifications_user ON tb_notifications(UserID);
CREATE INDEX IF NOT EXISTS idx_notifications_unread ON tb_notifications(IsRead, CreatedDate DESC);
```

Also update the tb_users CREATE TABLE to add:
```sql
NotificationsEnabled INTEGER DEFAULT 1
```

### 2. Increment Version

```csharp
private const int CURRENT_SCHEMA_VERSION = 2;
```

### 3. Launch Application

The migration runs automatically:
```
[DatabaseManager] Checking database schema version...
[DatabaseManager] Current schema version: 1, Target version: 2
[DatabaseManager] Database upgrade needed from version 1 to 2
[DatabaseManager] Database backup created: database.sqlite.backup_20240115_143022
[DatabaseManager] Loaded schema file (45832 bytes)
[DatabaseManager] Parsed 25 SQL statements
[DatabaseManager] Creating new table: tb_notifications
[DatabaseManager] Table 'tb_users' exists - checking for missing columns...
[DatabaseManager] Found 1 missing columns in 'tb_users'
[DatabaseManager] Added column 'NotificationsEnabled' to 'tb_users'
[DatabaseManager] Schema application complete: 1 new tables, 2 indexes applied
[DatabaseManager] Database successfully upgraded to version 2
```

## SQL File Requirements

Your `database.sqlite.sql` must follow these guidelines:

1. **Use CREATE IF NOT EXISTS**:
   ```sql
   CREATE TABLE IF NOT EXISTS tb_myTable (...)
   ```

2. **Use CREATE INDEX IF NOT EXISTS**:
   ```sql
   CREATE INDEX IF NOT EXISTS idx_myTable_field ON tb_myTable(field);
   ```

3. **Provide DEFAULT values for new columns**:
   ```sql
   NewColumn TEXT DEFAULT ''
   NewFlag INTEGER DEFAULT 0
   ```

4. **Skip INSERT statements** during migration - they're for initial setup only

## Advanced: Complex Migrations

For operations that can't be automated (renaming, type changes, removing columns), use custom migrations:

```csharp
case 3:
    // First, let SQL file create/update what it can
    // Then apply complex changes
    RenameUserEmailColumn(conn, tx);
    break;

private static void RenameUserEmailColumn(SqliteConnection conn, SqliteTransaction tx)
{
    // Complex table restructuring logic here
    AppDebug.Log("DatabaseManager", "Custom migration completed");
}
```

## Troubleshooting

### Migration Failed

1. Check the application logs for error details
2. Find the backup file (most recent `database.sqlite.backup_*`)
3. Restore the backup: Delete `database.sqlite` and rename backup to `database.sqlite`
4. Fix the SQL file or add custom migration
5. Increment version and try again

### Database Version Mismatch

If the database version is newer than expected:
- The application will log a warning but continue
- Some features may not work correctly
- Update the application to the latest version

### Manual Version Reset

To manually reset the schema version (use with extreme caution):

```sql
UPDATE tb_settings SET value = '0' WHERE key = 'schema_version';
-- Or delete it entirely:
DELETE FROM tb_settings WHERE key = 'schema_version';
```

This will cause all migrations to re-run on next startup.

### Column Can't Be Added

If you see errors like "Cannot add column" it's usually because:
- The column lacks a DEFAULT value or NULL constraint
- The column is defined as PRIMARY KEY (can't add via ALTER TABLE)

**Solution**: Add DEFAULT value in SQL file:
```sql
NewColumn TEXT DEFAULT ''
NewNumber INTEGER DEFAULT 0
NewDate TEXT DEFAULT (datetime('now'))
```

### Table Already Exists Error

This shouldn't happen if you use `CREATE TABLE IF NOT EXISTS`. If it does:
- Verify your SQL file uses `IF NOT EXISTS`
- Check for duplicate table definitions in SQL file

## Current Schema Version History

- **Version 0**: Legacy database (no version tracking)
- **Version 1**: Current production schema
  - All core tables (users, permissions, audit logs, maps, chat, proxy, player records)
  - All indexes defined in database.sqlite.sql
  - Version tracking system implemented

*Update this section when incrementing schema version*

## Tips for Maintaining database.sqlite.sql

1. **Keep it formatted** - Use consistent indentation and spacing
2. **Always use IF NOT EXISTS** - Makes the file reapplication-safe
3. **Comment your changes** - Add SQL comments explaining new additions
4. **Test the SQL file** - You can test it by creating a new blank database:
   ```bash
   sqlite3 test.db < database.sqlite.sql
   ```
5. **Include all structure** - Tables, indexes, constraints
6. **Exclude data** - Don't include INSERT statements for regular data (only reference data like default maps)

## Reference: database.sqlite.sql Location

```
BHD-ServerManager/
??? Databases/
    ??? database.sqlite         (actual database file)
    ??? database.sqlite.sql     (schema source file - EDIT THIS)
    ??? database.sqlite.backup_* (automatic backups)
```
