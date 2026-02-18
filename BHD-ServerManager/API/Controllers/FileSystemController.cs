using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/filesystem")]
[Authorize]
public class FileSystemController : ControllerBase
{
    /// <summary>
    /// Get list of available drives
    /// </summary>
    [HttpGet("drives")]
    public ActionResult<DirectoryListingResponse> GetDrives()
    {
        if(!HasPermission("profile")) return Forbid();
        try
        {
            var drives = DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(d => d.Name)
                .ToList();

            return Ok(new DirectoryListingResponse
            {
                Success = true,
                Message = "Drives retrieved successfully",
                Drives = drives,
                CurrentPath = string.Empty,
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
            // Default to drives if no path specified
            if (string.IsNullOrWhiteSpace(request.Path))
            {
                return GetDrives();
            }

            var dirInfo = new DirectoryInfo(request.Path);

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

            var drives = DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(d => d.Name)
                .ToList();

            return Ok(new DirectoryListingResponse
            {
                Success = true,
                Message = "Directory listed successfully",
                CurrentPath = dirInfo.FullName,
                ParentPath = parentPath,
                Entries = entries.OrderBy(e => !e.IsDirectory).ThenBy(e => e.Name).ToList(),
                Drives = drives
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
            if (string.IsNullOrWhiteSpace(request.Path))
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Path cannot be empty"
                });
            }

            bool exists = Directory.Exists(request.Path);

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

        string fileName = file != null ? Path.GetFileName(file.FileName) : string.Empty;
        bool success = false;
        string message = string.Empty;
        int count = 0;
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

            if (file == null || file.Length == 0)
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
            var destPath = Path.Combine(theInstance.profileServerPath, fileName);
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

            var filePath = Path.Combine(theInstance.profileServerPath, fileName);
        
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

        string fileNames = string.Join(", ", request.FileNames ?? new List<string>());
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
            
            if (mapInstance != null && mapInstance.Playlists != null)
            {
                foreach (var fileName in request.FileNames!)
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

            foreach (var fileName in request.FileNames!)
            {
                var filePath = Path.Combine(theInstance.profileServerPath, fileName);
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
                    var destFile = Path.Combine(destinationPath, entry.Name);
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
