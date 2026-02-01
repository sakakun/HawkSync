using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HawkSyncShared;
using HawkSyncShared.DTOs;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MapPlaylistController : ControllerBase
{
    [HttpGet("available-maps")]
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

    [HttpGet("playlists")]
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

    [HttpGet("playlist/{id}")]
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

        var maps = playlist.Maps.Select(m => new HawkSyncShared.ObjectClasses.mapFileInfo
        {
            MapID = m.MapID,
            MapName = m.MapName,
            MapFile = m.MapFile,
            ModType = m.ModType,
            MapType = m.MapType
        }).ToList();

        var result = mapInstanceManager.SavePlaylist(playlist.PlaylistID, maps);

        TriggerServerUIReload();

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

        var saveResult = mapInstanceManager.SavePlaylist(playlist.PlaylistID, playlist.Maps.Select(m => new HawkSyncShared.ObjectClasses.mapFileInfo
        {
            MapID = m.MapID,
            MapName = m.MapName,
            MapFile = m.MapFile,
            ModType = m.ModType,
            MapType = m.MapType
        }).ToList());

        if (!saveResult.Success)
            return Ok(new PlaylistCommandResult { Success = false, Message = saveResult.Message });

        var activateResult = mapInstanceManager.SetActivePlaylist(playlist.PlaylistID);
        TriggerServerUIReload();
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

        // Overwrite playlist with imported maps
        var result = mapInstanceManager.SavePlaylist(playlist.PlaylistID, playlist.Maps.Select(m => new HawkSyncShared.ObjectClasses.mapFileInfo
        {
            MapID = m.MapID,
            MapName = m.MapName,
            MapFile = m.MapFile,
            ModType = m.ModType,
            MapType = m.MapType
        }).ToList());

        TriggerServerUIReload();

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

    // Server memory actions
    [HttpPost("server/skip-map")]
    public ActionResult<PlaylistCommandResult> SkipMap()
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.SkipMap();
        return Ok(new PlaylistCommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("server/score-map")]
    public ActionResult<PlaylistCommandResult> ScoreMap()
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.ScoreMap();
        return Ok(new PlaylistCommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("server/play-next")]
    public ActionResult<PlaylistCommandResult> PlayNext([FromBody] int mapIndex)
    {
        if(!HasPermission("maps")) return Forbid();

        var result = mapInstanceManager.SetNextMap(mapIndex);
        return Ok(new PlaylistCommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("refresh-available-maps")]
    public ActionResult<List<MapDTO>> RefreshAvailableMaps()
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

    private void TriggerServerUIReload()
    {
        // Trigger UI update on server manager via invoke
        var serverUI = Program.ServerManagerUI;
        if (serverUI != null)
        {
            serverUI.Invoke(() =>
            {
                serverUI.MapsTab?.actionClick_refeshMapLists(null!,null!);
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

}