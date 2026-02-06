using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HawkSyncShared;
using BHD_ServerManager.API.Services;
using HawkSyncShared.DTOs.API;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SnapshotController : ControllerBase
{
    [HttpGet]
    public ActionResult<ServerSnapshot> GetSnapshot()
    {
        if (CommonCore.theInstance == null)
        {
            return StatusCode(503, "Server not initialized");
        }

        var snapshot = InstanceMapper.CreateSnapshot(
            CommonCore.theInstance,
            CommonCore.instancePlayers ?? new(),
            CommonCore.instanceChat ?? new(),
            CommonCore.instanceBans ?? new(),
            CommonCore.instanceMaps ?? new(),
            CommonCore.instanceAdmin ?? new(),
            CommonCore.instanceStats ?? new()
        );

        return Ok(snapshot);
    }
}