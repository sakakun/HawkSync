# NetLimiter Proxy Checker

A tool for monitoring and managing network connections, focusing on identifying and handling VPN or proxy connections.

## Requirements
- NetLimiter (Latest Version)
- .NET Framework 4.6.2

## Feature Actions
- initConnection: Initialize connection to NetLimiter.
- endProgram: Terminate the program and clean up resources.
- getAppID: Retrieve the application ID for a specified application. 
  - Parameters: Application Path
- getConnections: List all current network connections for a specified application.
  - Parameters: Application ID
- getFilterObject: Retrieve filter objects associated with a specified name.
  - Parameters: Filter Name
- addIPToFilter: Add an IP address to a specified filter.
  - Parameters: Filter Name, IP Address
- removeIPFromFilter: Remove an IP address from a specified filter.
  - Parameters: Filter Name, IP Address
- setConnectionLimit: Set the connection limit for a specified application.
  - Parameters: Connection Limit
- enableConnectionLimit: Enable or disable monitoring for the number of connection for the specified application.
  - Parameters: Enable/Disable
  - A hook in the "getConnections" action to enforce the connection limit and trigger addIPToFilter if the limit is exceeded.

- Application to use a Named Pipe for communication with other applications.  The program will be started by another application and will listen for commands via the Named Pipe.  The connection to Netlimiter will be maintain until program closes.
  - Pipe Name: NetLimiterPipe
