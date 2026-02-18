using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/maps")]
[Authorize]
public class MapPlaylistController : ControllerBase
{
    [HttpGet("list/available")]
    public ActionResult<List<MapDTO>> GetAvailableMaps()
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.LoadAvailableMaps();
        var allMaps = result.DefaultMaps.Concat(result.CustomMaps)
            .Select(m => new MapDTO
            {
                MapID = m.MapID,
                MapName = m.MapName,
                MapFile = m.MapFile,
                ModType = m.ModType,
                MapType = m.MapType
            }).ToList();
        return Ok(allMaps);
    }

    [HttpGet("list/playlists")]
    public ActionResult<AllPlaylistsDTO> GetAllPlaylists()
    {
        if(!HasPermission("maps")) return Forbid();

        var instance = CommonCore.instanceMaps!;
        var dto = new AllPlaylistsDTO
        {
            ActivePlaylist = instance.ActivePlaylist,
            SelectedPlaylist = instance.SelectedPlaylist,
            Playlists = instance.Playlists.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(m => new MapDTO
                {
                    MapID = m.MapID,
                    MapName = m.MapName,
                    MapFile = m.MapFile,
                    ModType = m.ModType,
                    MapType = m.MapType
                }).ToList()
            )
        };
        return Ok(dto);
    }

    [HttpGet("list/playlist/{id}")]
    public ActionResult<PlaylistDTO> GetPlaylist(int id)
    {
        if(!HasPermission("maps")) return Forbid();

        var (success, maps, error) = mapInstanceManager.GetPlaylistMaps(id);
        if (!success)
            return BadRequest(new PlaylistCommandResult { Success = false, Message = error });

        return Ok(new PlaylistDTO
        {
            PlaylistID = id,
            Maps = maps.Select(m => new MapDTO
            {
                MapID = m.MapID,
                MapName = m.MapName,
                MapFile = m.MapFile,
                ModType = m.ModType,
                MapType = m.MapType
            }).ToList()
        });
    }

    [HttpPost("playlist/save")]
    public ActionResult<PlaylistCommandResult> SavePlaylist([FromBody] PlaylistDTO playlist)
    {
        if(!HasPermission("maps")) return Forbid();

        var maps = playlist.Maps.Select(m => new MapObject
        {
            MapID = m.MapID,
            MapName = m.MapName,
            MapFile = m.MapFile,
            ModType = m.ModType,
            MapType = m.MapType
        }).ToList();

        var result = mapInstanceManager.SavePlaylist(playlist.PlaylistID, maps);
        TriggerServerUIReload();
        LogMapPlaylistAction(
            "SavePlaylist",
            $"Saved playlist {playlist.PlaylistID}",
            result.Success,
            result.Message
        );
        return Ok(new PlaylistCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("playlist/set-active")]
    public ActionResult<PlaylistCommandResult> SetActivePlaylist([FromBody] PlaylistDTO playlist)
    {
        if(!HasPermission("maps")) return Forbid();

        var saveResult = mapInstanceManager.SavePlaylist(playlist.PlaylistID, playlist.Maps.Select(m => new MapObject
        {
            MapID = m.MapID,
            MapName = m.MapName,
            MapFile = m.MapFile,
            ModType = m.ModType,
            MapType = m.MapType
        }).ToList());

        if (!saveResult.Success)
        {
            LogMapPlaylistAction(
                "SetActivePlaylist",
                $"Failed to save playlist {playlist.PlaylistID} before activating",
                false,
                saveResult.Message
            );
            return Ok(new PlaylistCommandResult { Success = false, Message = saveResult.Message });
        }

        var activateResult = mapInstanceManager.SetActivePlaylist(playlist.PlaylistID);
        TriggerServerUIReload();
        LogMapPlaylistAction(
            "SetActivePlaylist",
            $"Set active playlist to {playlist.PlaylistID}",
            activateResult.Success,
            activateResult.Message
        );
        return Ok(new PlaylistCommandResult
        {
            Success = activateResult.Success,
            Message = activateResult.Message
        });
    }

    [HttpPost("playlist/import")]
    public ActionResult<PlaylistCommandResult> ImportPlaylist([FromBody] PlaylistDTO playlist)
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.SavePlaylist(playlist.PlaylistID, playlist.Maps.Select(m => new MapObject
        {
            MapID = m.MapID,
            MapName = m.MapName,
            MapFile = m.MapFile,
            ModType = m.ModType,
            MapType = m.MapType
        }).ToList());
        TriggerServerUIReload();
        LogMapPlaylistAction(
            "ImportPlaylist",
            $"Imported playlist {playlist.PlaylistID}",
            result.Success,
            result.Message
        );
        return Ok(new PlaylistCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpGet("playlist/export/{id}")]
    public ActionResult<PlaylistDTO> ExportPlaylist(int id)
    {
        if(!HasPermission("maps")) return Forbid();

        var (success, maps, error) = mapInstanceManager.GetPlaylistMaps(id);
        if (!success)
        {
            LogMapPlaylistAction(
                "ExportPlaylist",
                $"Failed to export playlist {id}",
                false,
                error
            );
            return BadRequest(new PlaylistCommandResult { Success = false, Message = error });
        }
        LogMapPlaylistAction(
            "ExportPlaylist",
            $"Exported playlist {id}",
            true,
            "Export successful"
        );
        return Ok(new PlaylistDTO
        {
            PlaylistID = id,
            Maps = maps.Select(m => new MapDTO
            {
                MapID = m.MapID,
                MapName = m.MapName,
                MapFile = m.MapFile,
                ModType = m.ModType,
                MapType = m.MapType
            }).ToList()
        });
    }

    // Map Actions
    [HttpPost("server/skip-map")]
    public ActionResult<PlaylistCommandResult> SkipMap()
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.SkipMap();
        LogMapPlaylistAction(
            "SkipMap",
            "Skipped current map in playlist",
            result.Success,
            result.Message
        );
        return Ok(new PlaylistCommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("server/score-map")]
    public ActionResult<PlaylistCommandResult> ScoreMap()
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.ScoreMap();
        LogMapPlaylistAction(
            "ScoreMap",
            "Scored current map in playlist",
            result.Success,
            result.Message
        );
        return Ok(new PlaylistCommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("server/play-next")]
    public ActionResult<PlaylistCommandResult> PlayNext([FromBody] int mapIndex)
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.SetNextMap(mapIndex);
        LogMapPlaylistAction(
            "PlayNext",
            $"Set next map in playlist to index {mapIndex}",
            result.Success,
            result.Message
        );
        return Ok(new PlaylistCommandResult { Success = result.Success, Message = result.Message });
    }

    private void TriggerServerUIReload()
    {
        // Trigger UI update on server manager via invoke
        var serverUI = Program.ServerManagerUI;
        if (serverUI != null)
        {
            serverUI.Invoke(() =>
            {
                serverUI.MapsTab?.OnRefreshMapLists(null!,null!);
            });
        }
    }

    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        AppDebug.Log("MapPlaylistController", $"Checking user permissions, {permission} from user { User.Identity!.Name}");
        AppDebug.Log("MapPlaylistController", $"User permissions: {string.Join(", ", permissions)}");
        AppDebug.Log("MapPlaylistController", $"Has Permission: {permissions.Contains(permission).ToString()}");
        return permissions.Contains(permission);
    }

    private void LogMapPlaylistAction(string actionType, string description, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.Map,
            actionType: actionType,
            description: description,
            targetType: "MapPlaylist",
            targetId: null,
            targetName: null,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

}