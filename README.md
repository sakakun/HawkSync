## HawkSync

### Features of the Server Manager

1. Server Control
- Start/Stop Server: Start or stop the game server, with status updates shown in the UI.
- Server Settings Management: Configure server path, ports, dedicated mode, passwords, and other core settings.
- Save/Load Settings: Save current server settings or import/export them from files.
2. Map Management
- Map Playlist Editing: Add, remove, reorder, and save maps in the current playlist (up to 128 maps).
- Map Filtering: Filter available maps by game type.
- Import/Export Playlists: Import map playlists from files or export the current playlist.
- Reset/Refresh Maps: Reset available maps to default or refresh the list.
3. Ban Management
- Add/Remove Bans: Add bans by player name or IP, with submask support.
- Ban List Editing: Remove bans via UI, including handling records that exist in both name and IP lists.
4. Chat Management
- Send Chat Messages: Send messages to the server, with support for channel prefixes and player name substitution.
- Auto/Slap Messages: Add, remove, and manage automated and slap messages for in-game chat.
5. Admin Management
- Admin Accounts: Add, edit, or delete admin accounts with role selection.
- Admin List: View and manage the list of admin users.
6. Game Settings
- Weapon Restrictions: Enable/disable specific weapons for gameplay.
- Role Restrictions: (If implemented) Restrict certain player roles.
- Game Options: Configure friendly fire, ping limits, and other gameplay options.
7. Stats & Web Integration
- Babstats Integration: Enable/disable web stats, set update/report intervals, and test server connectivity.
- Announcements: Enable/disable web stats announcements.
8. Remote Access
- Remote Feature: All above features are accessible remotely via the Remote client, which mirrors the Server Manager’s functionality over a secure connection.
9. User Interface
- Dynamic Controls: UI elements enable/disable based on server status.
- Data Grids: Interactive grids for maps, bans, admins, and chat messages.
- Status Bar: Real-time server status and information display.
---
Note:
The Remote client is a feature that provides all the above Server Manager functionality from a different machine, using secure communication.

### Installation
1. Download the latest release from the Releases page.
- HawkSync-Installer for an all-in-one installer which you can pick which components you want to install.
- Extract the HawkSync-ServerManager.zip to a folder of your choice.
- Extract the HawkSync-Remote.zip to a folder of your choice.

### Requirements
- .NET 8.0 Desktop Runtime (or higher).
- Webview2 Runtime. (Windows 10 & 11 with Edge installed).

### Roadmap

#### Log Management Improvements
- Move all logs to a seperate tab in the UI.
- Move all "saved" logs to a seperate folder.

#### Map Management Improvements
- Expand map filtering options to allow to type a map name to filter.
- Add a "Map Details" view to show more information about each map.
- Add and Remove Maps from the Game Server directly from the UI.
- Review Map Bitmap handling to ensure it works correctly.

#### Game Server Profile Management
- Add tab for managing game server profile.
- Change Game Server Profile Path.
- File Management for Game Server itself.
- Additional "switches" for the game server executable.

#### User Management Improvements
- Permission system for the users.

### Known Issues
- No issues at this time.

### Fixed Issues
 - 07/30/25: The Server Manager may lock up when trying to load a large number of logs.