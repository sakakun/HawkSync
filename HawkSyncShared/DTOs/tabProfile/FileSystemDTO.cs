namespace HawkSyncShared.DTOs.tabProfile;

/// <summary>
/// File system entry (file or directory)
/// </summary>
public class FileSystemEntry
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
}

/// <summary>
/// Response for directory listing
/// </summary>
public class DirectoryListingResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string CurrentPath { get; set; } = string.Empty;
    public string? ParentPath { get; set; }
    public List<FileSystemEntry> Entries { get; set; } = new();
    public List<string> Drives { get; set; } = new();
}

/// <summary>
/// Request for directory listing
/// </summary>
public class DirectoryListingRequest
{
    public string? Path { get; set; }
    public string? FileFilter { get; set; } // e.g., "*.pff"
}