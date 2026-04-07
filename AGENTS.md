# AGENTS.md — HawkSync Codebase Guide

## Project Overview
HawkSync is a **Delta Force: Black Hawk Down** game-server manager built on **.NET 10 WinForms (x64, Windows-only)**. It consists of three projects that all target `net10.0-windows`:

| Project | Role |
|---|---|
| `BHD-ServerManager` | Main WinForms app — manages the `dfbhd.exe` process, hosts the embedded API |
| `HawkSyncShared` | Shared library — all instance data objects, DTOs, and common utilities |
| `RemoteClient` | Companion WinForms app — connects to the embedded API over HTTP/SignalR |

---

## Core Architecture

### Instance Pattern (Central State)
All live server state is held in **instance objects** registered as static properties on `CommonCore` (`HawkSyncShared/CommonCore.cs`):

```csharp
CommonCore.theInstance       // Server config, status, game settings
CommonCore.instancePlayers   // Live player list
CommonCore.instanceChat      // Chat messages, auto-message state
CommonCore.instanceMaps      // Map playlist, current map
CommonCore.instanceBans      // Ban/whitelist records
CommonCore.instanceAdmin     // Admin users, active sessions
CommonCore.instanceStats     // Stats, Babstats integration
```

**Instances are plain data objects** (defined in `HawkSyncShared/Instances/`). Fields marked `[JsonIgnore]` are server-only runtime state and are never sent to RemoteClient.

### InstanceManagers (Business Logic)
Each instance has a corresponding static manager in `BHD-ServerManager/Classes/InstanceManagers/` (e.g., `theInstanceManager`, `playerInstanceManager`). Managers read/write instances and call `ServerMemory` or `DatabaseManager`. **Never put game logic directly in instance classes.**

### Tickers (Polling Loops)
All periodic work is driven by `CommonCore.Ticker` (`HawkSyncShared/SupportClasses/Ticker.cs`). Tickers are registered in `theInstanceManager.InitializeTickers()`:

| Ticker name | Interval | Responsibility |
|---|---|---|
| `ServerManager` | 500 ms | API lifecycle management, server memory polling |
| `ChatManager` | 100 ms | Chat polling |
| `PlayerManager` | 1000 ms | Player list polling |
| `BanManager` | 1000 ms | Ban enforcement |
| `SessionCleanup` | 60 s | Stale SignalR session cleanup |

Ticker bodies guard against re-entry with `Interlocked.CompareExchange`. See `tickerServerManager.cs` for the canonical pattern.

### Server Memory (P/Invoke)
`BHD-ServerManager/Classes/GameManagement/ServerMemory.cs` uses `ReadProcessMemory` / `WriteProcessMemory` via P/Invoke to interact with `dfbhd.exe` at hardcoded offsets from `baseAddr = 0x400000`. The process handle is acquired when the server starts. All memory I/O is gated on `theInstance.instanceStatus != OFFLINE`.

---

## Embedded API (BHD-ServerManager → RemoteClient)
The API is an **ASP.NET Core host embedded inside the WinForms process** (`EmbeddedApiHost.cs`). It is started/stopped by `APICore` based on `theInstance.profileEnableRemote`, checked every 500 ms by `tickerServerManager`.

- **REST**: `http://0.0.0.0:<port>/api/[controller]` — controllers in `BHD-ServerManager/API/Controllers/`
- **SignalR**: `/hubs/server` — hub `ServerHub.cs`; broadcasts `ServerSnapshot` every 1 second via `InstanceBroadcastService`
- **Auth**: JWT (issuer `BHD.ServerManager`, audience `BHD.RemoteClient`); token passed as query param `access_token` for SignalR
- **DTOs**: defined in `HawkSyncShared/DTOs/API/` and per-tab folders — shared between server and client

Adding a new API controller: create it in `BHD-ServerManager/API/Controllers/`, decorate with `[ApiController]` and `[Authorize]`, follow the existing controller pattern. Add new snapshot fields to `InstanceMapper.CreateSnapshot()` and the `ServerSnapshot` DTO in `HawkSyncShared`.

---

## Database & Settings

### Schema
SQLite file at `BHD-ServerManager/Databases/database.sqlite` (copied to output via `.csproj`). `DatabaseManager` (`Classes/SupportClasses/DatabaseManager.cs`) is a static singleton that holds a single open connection for the lifetime of the process.

### Settings Workflow
All settings use a key-value store in `tb_settings`:

```csharp
ServerSettings.Get("gameMaxSlots", 50)    // reads DB, caches in memory
ServerSettings.Set("gameMaxSlots", value) // writes cache + DB
```

`ServerSettings` wraps `DatabaseManager.GetSetting/SetSetting` with an in-memory `Dictionary` cache.

### Schema Migrations
1. Edit `BHD-ServerManager/Databases/database.sqlite.sql` (always use `CREATE TABLE IF NOT EXISTS`)
2. Increment `CURRENT_SCHEMA_VERSION` in `DatabaseManager.cs`
3. For complex changes (column renames, type changes), add a `case N:` block to `ApplyCustomMigrations()` in `DatabaseManager.cs`

See `Databases/DATABASE_MIGRATION_GUIDE.md` for detailed instructions.

---

## Logging (`AppDebug`)
`HawkSyncShared/SupportClasses/AppDebug.cs` — a static logger with three modes:

| Mode | When |
|---|---|
| `Debug.WriteLine` | Debugger attached |
| File (`AppDebug.log` in exe dir) | App launched with `/debug` arg |
| Windows Event Log | Production errors (no debugger) |

Use: `AppDebug.Log("message", AppDebug.LogLevel.Info)` — caller info is captured automatically via `[CallerMemberName]` etc.

---

## Adding a New Feature — Checklist
1. **DTO** → `HawkSyncShared/DTOs/tabXxx/` (mark server-only fields `[JsonIgnore]`)
2. **Instance** → `HawkSyncShared/Instances/xxxInstance.cs` + register in `CommonCore.InitializeCore()`
3. **InstanceManager** → `BHD-ServerManager/Classes/InstanceManagers/xxxInstanceManager.cs`
4. **API controller** → `BHD-ServerManager/API/Controllers/XxxController.cs`
5. **Snapshot** → add field to `ServerSnapshot` DTO and `InstanceMapper.CreateSnapshot()`
6. **UI panel** (server) → `BHD-ServerManager/Forms/Panels/tabXxx.cs` (WinForms UserControl)
7. **Schema** → update `database.sqlite.sql` + bump `CURRENT_SCHEMA_VERSION` if DB changes needed

---

## Build & Debug
- Target: `net10.0-windows`, platform `x64` — build with Visual Studio or `dotnet build -r win-x64`
- The solution file is `HawkSync.sln`
- `database.sqlite` must exist in `BHD-ServerManager/Databases/` before first run (copied to output automatically)
- Launch with `/debug` flag to enable file-based logging to `AppDebug.log`
- When debugger is attached, `readAutoRes()` is called at start-delay (test hook in `tickerServerManager.cs`)

