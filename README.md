## HawkSync

### Features of the Server Manager


---
Note:
The Remote client is a feature that provides all the above Server Manager functionality from a different machine, using secure communication.

### Installation
Download the latest release from the Releases page.
- HawkSync-Installer for an all-in-one installer which you can pick which components you want to install. (Requires Run As Admin for Desktop Shortcut, no clue why atm.)
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
- Add Team Sabre Maps

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
