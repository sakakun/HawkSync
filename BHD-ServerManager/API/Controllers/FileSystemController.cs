using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabProfile;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }
}