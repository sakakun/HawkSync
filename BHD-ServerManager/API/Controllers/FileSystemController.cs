using ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace ServerManager.API.Controllers;

[ApiController]
[Route("api/filesystem")]
[Authorize]
public class FileSystemController : ControllerBase
{
    private static readonly char[] _dirSeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

    /// <summary>
    /// Get list of available drives
    /// </summary>
    [HttpGet("drives")]
    public ActionResult<DirectoryListingResponse> GetDrives()
    {
        if(!HasPermission("profile")) return Forbid();
        try
        {
            if (!TryGetServerRootPath(out var serverRootPath, out var errorMessage))
            {
                return Ok(new DirectoryListingResponse
                {
                    Success = false,
                    Message = errorMessage
                });
            }

            return Ok(new DirectoryListingResponse
            {
                Success = true,
                Message = "Server root retrieved successfully",
                Drives = new List<string> { serverRootPath },
                CurrentPath = serverRootPath,
                Entries = new List<FileSystemEntry>()
            });
        }
        catch (Exception ex)
        {
            return Ok(new DirectoryListingResponse
            {
                Success = false,
                Message = $"Error retrieving drives: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Get directory listing
    /// </summary>
    [HttpPost("list")]
    public ActionResult<DirectoryListingResponse> GetDirectoryListing([FromBody] DirectoryListingRequest request)
    {
        if(!HasPermission("profile")) return Forbid();

        try
        {
            if (!TryGetServerRootPath(out var serverRootPath, out var errorMessage))
            {
                return Ok(new DirectoryListingResponse
                {
                    Success = false,
                    Message = errorMessage
                });
            }

            // Default to server root if no path is specified.
            var requestedPath = string.IsNullOrWhiteSpace(request.Path) ? serverRootPath : request.Path;
            if (!TryResolvePathUnderRoot(serverRootPath, requestedPath!, allowAbsoluteInput: true, out var resolvedDirectoryPath))
            {
                LogPathTraversalBlocked("ListDirectory", requestedPath!);
                return Ok(new DirectoryListingResponse
                {
                    Success = false,
                    Message = "Access denied: path must be within the configured server path"
                });
            }

            var dirInfo = new DirectoryInfo(resolvedDirectoryPath);

            if (!dirInfo.Exists)
            {
                return Ok(new DirectoryListingResponse
                {
                    Success = false,
                    Message = "Directory does not exist"
                });
            }

            var entries = new List<FileSystemEntry>();

            // Add subdirectories
            try
            {
                foreach (var dir in dirInfo.GetDirectories())
                {
                    try
                    {
                        entries.Add(new FileSystemEntry
                        {
                            Name = dir.Name,
                            FullPath = dir.FullName,
                            IsDirectory = true,
                            Size = 0,
                            LastModified = dir.LastWriteTime
                        });
                    }
                    catch
                    {
                        // Skip directories we can't access
                    }
                }
            }
            catch
            {
                // Continue even if we can't list all directories
            }

            // Add files (with optional filter)
            try
            {
                var searchPattern = string.IsNullOrWhiteSpace(request.FileFilter) ? "*.*" : request.FileFilter;
                
                foreach (var file in dirInfo.GetFiles(searchPattern))
                {
                    try
                    {
                        entries.Add(new FileSystemEntry
                        {
                            Name = file.Name,
                            FullPath = file.FullName,
                            IsDirectory = false,
                            Size = file.Length,
                            LastModified = file.LastWriteTime
                        });
                    }
                    catch
                    {
                        // Skip files we can't access
                    }
                }
            }
            catch
            {
                // Continue even if we can't list all files
            }

            // Get parent directory
            string? parentPath = dirInfo.Parent?.FullName;
            if (!string.IsNullOrEmpty(parentPath) &&
                !IsPathUnderRoot(serverRootPath, parentPath))
            {
                parentPath = null;
            }

            return Ok(new DirectoryListingResponse
            {
                Success = true,
                Message = "Directory listed successfully",
                CurrentPath = dirInfo.FullName,
                ParentPath = parentPath,
                Entries = entries.OrderBy(e => !e.IsDirectory).ThenBy(e => e.Name).ToList(),
                Drives = new List<string> { serverRootPath }
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Ok(new DirectoryListingResponse
            {
                Success = false,
                Message = "Access denied to this directory"
            });
        }
        catch (Exception ex)
        {
            return Ok(new DirectoryListingResponse
            {
                Success = false,
                Message = $"Error listing directory: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Validate if a path exists
    /// </summary>
    [HttpPost("validate-path")]
    public ActionResult<CommandResult> ValidatePath([FromBody] DirectoryListingRequest request)
    {
        if(!HasPermission("profile")) return Forbid();

        try
        {
            if (!TryGetServerRootPath(out var serverRootPath, out var errorMessage))
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = errorMessage
                });
            }

            if (string.IsNullOrWhiteSpace(request.Path))
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Path cannot be empty"
                });
            }

            if (!TryResolvePathUnderRoot(serverRootPath, request.Path, allowAbsoluteInput: true, out var resolvedPath))
            {
                LogPathTraversalBlocked("ValidatePath", request.Path);
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Access denied: path must be within the configured server path"
                });
            }

            bool exists = Directory.Exists(resolvedPath);

            return Ok(new CommandResult
            {
                Success = exists,
                Message = exists ? "Path exists" : "Path does not exist"
            });
        }
        catch (Exception ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Error validating path: {ex.Message}"
            });
        }

    }
    
    // ================================================================================
    // FILE MANAGER OPERATIONS
    // ================================================================================

    /// <summary>
    /// Get list of files in profileServerPath
    /// </summary>
    [HttpGet("files")]
    public ActionResult<FileListResponse> GetFiles()
    {
        if(!HasPermission("profile")) return Forbid();

        try
        {
            var theInstance = CommonCore.theInstance;
            if (theInstance == null || string.IsNullOrEmpty(theInstance.profileServerPath))
            {
                return Ok(new FileListResponse
                {
                    Success = false,
                    Message = "Server path is not configured"
                });
            }

            if (!Directory.Exists(theInstance.profileServerPath))
            {
                return Ok(new FileListResponse
                {
                    Success = false,
                    Message = $"Directory not found: {theInstance.profileServerPath}"
                });
            }

            var allowedExtensions = new[] { ".bms", ".mis", ".til" };
            var files = Directory.GetFiles(theInstance.profileServerPath)
                .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
                .OrderBy(f => Path.GetFileName(f))
                .Select(f =>
                {
                    var fileInfo = new FileInfo(f);
                    return new FileEntry
                    {
                        FileName = fileInfo.Name,
                        Size = fileInfo.Length,
                        LastModified = fileInfo.LastWriteTime
                    };
                })
                .ToList();

            return Ok(new FileListResponse
            {
                Success = true,
                Message = "Files retrieved successfully",
                Files = files
            });
        }
        catch (Exception ex)
        {
            return Ok(new FileListResponse
            {
                Success = false,
                Message = $"Error retrieving files: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Upload a file
    /// </summary>
    [HttpPost("upload")]
    [RequestSizeLimit(100_000_000)] // 100MB limit
    public async Task<ActionResult<FileOperationResponse>> UploadFile(IFormFile file)
    {
        if(!HasPermission("profile")) return Forbid();

        string fileName = Path.GetFileName(file.FileName);
        bool success = false;
        string message = string.Empty;
        int count;
        try
        {
            var theInstance = CommonCore.theInstance;
            if (theInstance == null || string.IsNullOrEmpty(theInstance.profileServerPath))
            {
                message = "Server path is not configured";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            if (!Directory.Exists(theInstance.profileServerPath))
            {
                message = $"Directory not found: {theInstance.profileServerPath}";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            if (file.Length == 0)
            {
                message = "No file provided";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            var extension = Path.GetExtension(fileName).ToLower();
            var allowedExtensions = new[] { ".bms", ".mis", ".til", ".zip" };

            if (!allowedExtensions.Contains(extension))
            {
                message = $"File type not allowed: {fileName}. Allowed types: .bms, .mis, .til, .zip";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            // Handle ZIP files
            if (extension == ".zip")
            {
                int extractedCount = await ExtractZipFile(file, theInstance.profileServerPath);
                success = true;
                message = $"{extractedCount} file(s) extracted successfully";
                count = extractedCount;
                return Ok(new FileOperationResponse { Success = success, Message = message, Count = count });
            }

            // Handle individual files
            if (!TryResolvePathUnderRoot(theInstance.profileServerPath, fileName, allowAbsoluteInput: false, out var destPath))
            {
                LogPathTraversalBlocked("UploadFile", fileName);
                message = "Access denied: destination must be within the configured server path";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            using (var stream = new FileStream(destPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            success = true;
            message = "File uploaded successfully";
            count = 1;
            return Ok(new FileOperationResponse { Success = success, Message = message, Count = count });
        }
        catch (Exception ex)
        {
            message = $"Error uploading file: {ex.Message}";
            return Ok(new FileOperationResponse { Success = false, Message = message });
        }
        finally
        {
            LogFileAction("UploadFile", fileName, success, message);
        }
    }

    /// <summary>
    /// Download a file
    /// </summary>
    [HttpGet("download/{fileName}")]
    public ActionResult DownloadFile(string fileName)
    {
        if(!HasPermission("profile")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            var theInstance = CommonCore.theInstance;
            if (theInstance == null || string.IsNullOrEmpty(theInstance.profileServerPath))
            {
                message = "Server path is not configured";
                return BadRequest(message);
            }

            if (!TryResolvePathUnderRoot(theInstance.profileServerPath, fileName, allowAbsoluteInput: false, out var filePath))
            {
                LogPathTraversalBlocked("DownloadFile", fileName);
                message = "Access denied: file path must be within the configured server path";
                return BadRequest(message);
            }
        
            if (!System.IO.File.Exists(filePath))
            {
                message = "File not found";
                return NotFound(message);
            }

            var allowedExtensions = new[] { ".bms", ".mis", ".til" };
            var extension = Path.GetExtension(fileName).ToLower();
        
            if (!allowedExtensions.Contains(extension))
            {
                message = "File type not allowed";
                return BadRequest(message);
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            success = true;
            message = "File downloaded successfully";
            return File(memory, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            message = $"Error downloading file: {ex.Message}";
            return BadRequest(message);
        }
        finally
        {
            LogFileAction("DownloadFile", fileName, success, message);
        }
    }

    /// <summary>
    /// Delete files
    /// </summary>
    [HttpPost("delete")]
    public ActionResult<FileOperationResponse> DeleteFiles([FromBody] FileDeleteRequest request)
    {
        if(!HasPermission("profile")) return Forbid();

        string fileNames = string.Join(", ", request.FileNames);
        bool success = false;
        string message = string.Empty;
        int deletedCount = 0;
        try
        {
            var theInstance = CommonCore.theInstance;
            if (theInstance == null || string.IsNullOrEmpty(theInstance.profileServerPath))
            {
                message = "Server path is not configured";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            // Check if any .bms files are in use by playlists
            var filesInUse = new List<string>();
            var mapInstance = CommonCore.instanceMaps;
            
            if (mapInstance != null)
            {
                foreach (var fileName in request.FileNames)
                {
                    var extension = Path.GetExtension(fileName).ToLower();
                    if (extension == ".bms")
                    {
                        // Check playlists 1-5
                        for (int i = 1; i <= 5; i++)
                        {
                            if (mapInstance.Playlists.TryGetValue(i, out var playlist))
                            {
                                if (playlist.Any(map => string.Equals(map.MapFile, fileName, StringComparison.OrdinalIgnoreCase)))
                                {
                                    filesInUse.Add(fileName);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (filesInUse.Count > 0)
            {
                message = $"The following map files cannot be deleted because they are in use by playlists: {string.Join(", ", filesInUse)}";
                return Ok(new FileOperationResponse { Success = false, Message = message });
            }

            foreach (var fileName in request.FileNames)
            {
                if (!TryResolvePathUnderRoot(theInstance.profileServerPath, fileName, allowAbsoluteInput: false, out var filePath))
                {
                    LogPathTraversalBlocked("DeleteFiles", fileName);
                    message = $"Access denied for '{fileName}': path must be within the configured server path";
                    return Ok(new FileOperationResponse { Success = false, Message = message });
                }

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    deletedCount++;
                }
            }

            success = true;
            message = $"{deletedCount} file(s) deleted successfully";
            return Ok(new FileOperationResponse { Success = success, Message = message, Count = deletedCount });
        }
        catch (Exception ex)
        {
            message = $"Error deleting files: {ex.Message}";
            return Ok(new FileOperationResponse { Success = false, Message = message });
        }
        finally
        {
            LogFileAction("DeleteFiles", fileNames, success, message);
        }
    }

    private async Task<int> ExtractZipFile(IFormFile zipFile, string destinationPath)
    {
        int extractedCount = 0;
        var allowedExtensions = new[] { ".bms", ".mis", ".til" };

        using (var zipStream = zipFile.OpenReadStream())
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
        {
            foreach (var entry in archive.Entries)
            {
                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                var extension = Path.GetExtension(entry.Name).ToLower();
                if (allowedExtensions.Contains(extension))
                {
                    if (!TryResolvePathUnderRoot(destinationPath, entry.FullName, allowAbsoluteInput: false, out var destFile))
                    {
                        LogPathTraversalBlocked("ExtractZipFile", entry.FullName);
                        continue;
                    }

                    var parentDir = Path.GetDirectoryName(destFile);
                    if (!string.IsNullOrEmpty(parentDir))
                    {
                        Directory.CreateDirectory(parentDir);
                    }

                    entry.ExtractToFile(destFile, overwrite: true);
                    extractedCount++;
                }
            }
        }

        return extractedCount;
    }

    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

    private bool TryGetServerRootPath(out string serverRootPath, out string errorMessage)
    {
        serverRootPath = string.Empty;
        errorMessage = string.Empty;

        var theInstance = CommonCore.theInstance;
        if (theInstance == null || string.IsNullOrWhiteSpace(theInstance.profileServerPath))
        {
            errorMessage = "Server path is not configured";
            return false;
        }

        serverRootPath = Path.GetFullPath(theInstance.profileServerPath);
        if (!Directory.Exists(serverRootPath))
        {
            errorMessage = $"Directory not found: {serverRootPath}";
            return false;
        }

        return true;
    }

    private static bool TryResolvePathUnderRoot(string serverRootPath, string inputPath, bool allowAbsoluteInput, out string resolvedPath)
    {
        resolvedPath = string.Empty;

        if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(inputPath))
        {
            return false;
        }

        var candidatePath = allowAbsoluteInput && Path.IsPathRooted(inputPath)
            ? inputPath
            : Path.Combine(serverRootPath, inputPath);

        try
        {
            var candidateFullPath = Path.GetFullPath(candidatePath);
            if (!IsPathUnderRoot(serverRootPath, candidateFullPath))
            {
                return false;
            }

            resolvedPath = candidateFullPath;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsPathUnderRoot(string serverRootPath, string candidatePath)
    {
        var normalizedRoot = Path.GetFullPath(serverRootPath).TrimEnd(_dirSeparators);
        var normalizedCandidate = Path.GetFullPath(candidatePath).TrimEnd(_dirSeparators);

        if (normalizedCandidate.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var rootWithSeparator = normalizedRoot + Path.DirectorySeparatorChar;
        return normalizedCandidate.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase);
    }

    private void LogPathTraversalBlocked(string operation, string attemptedPath)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.System,
            actionType: AuditAction.PathTraversalBlocked,
            description: $"Blocked path traversal attempt during {operation}",
            targetType: "FileSystem",
            targetId: null,
            targetName: attemptedPath,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: false,
            errorMessage: "Requested path is outside the configured server path"
        );
    }

    private void LogFileAction(string actionType, string fileName, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.System, // or AuditCategory.Profile if you have one for file ops
            actionType: actionType,
            description: $"{actionType}: {fileName}",
            targetType: "File",
            targetId: null,
            targetName: fileName,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

}
