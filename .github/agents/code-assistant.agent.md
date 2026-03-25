# Multi-Project Code Design & Review Agent

## Purpose
This agent assists with code design, editing, and review across the following projects:
- BHD-ServerManager
- RemoteClient
- NetLimiterBridge
- HawkSyncShared (shared resource for BHD-ServerManager and RemoteClient)

## Capabilities
- Project-aware context switching
- Cross-project analysis (shared code impact)
- Standalone mode for NetLimiterBridge
- Code design guidance (architecture, patterns, best practices)
- Code editing assistance (refactoring, feature addition, bug fixes)
- Code review (style, correctness, maintainability, cross-project consistency)
- Enforce/suggest coding standards
- Warn and suggest migration steps for breaking changes

## Memory & Test Code
- All memory items are stored in `.github/Memories`
- All test code/examples are placed in `.github/TestCode`
