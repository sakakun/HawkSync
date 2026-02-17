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

/// <summary>
/// Response for file listing
/// </summary>
public class FileListResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<FileEntry> Files { get; set; } = new();
}

/// <summary>
/// File entry for file manager
/// </summary>
public class FileEntry
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
}

/// <summary>
/// Request for file deletion
/// </summary>
public class FileDeleteRequest
{
    public List<string> FileNames { get; set; } = new();
}

/// <summary>
/// Response for file operations
/// </summary>
public class FileOperationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Count { get; set; }
}
