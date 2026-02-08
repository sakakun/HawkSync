using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabBans;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[Route("api/ban")]
public class BanController : ControllerBase
{
    [HttpPost("save-blacklist")]
    public ActionResult<BanRecordSaveResult> SaveBlacklistRecord([FromBody] BanDTOs req)
    {
        if (!HasPermission("bans")) return Forbid();

        IPAddress? ip = null;
        if (req.IsIP && !string.IsNullOrWhiteSpace(req.IPAddress))
        {
            if (!IPAddress.TryParse(req.IPAddress, out ip))
                return BadRequest(new BanRecordSaveResult { Success = false, Message = "Invalid IP address." });
        }

        BanRecordSaveResult result;

        if (req.IsName && req.IsIP)
        {
            if (req.NameRecordID == null && req.IPRecordID == null)
            {
                // Use DualRecordResult for both-record creation
                var dualResult = banInstanceManager.AddBlacklistBothRecords(
                    req.PlayerName!, ip!, req.SubnetMask ?? 32, req.BanDate, req.ExpireDate, req.RecordType, req.Notes, req.IgnoreValidation
                );
                result = new BanRecordSaveResult
                {
                    Success = dualResult.Success,
                    Message = dualResult.Message,
                    NameRecordID = dualResult.NameRecordID,
                    IPRecordID = dualResult.IPRecordID
                };
            }
            else
            {
                result = new BanRecordSaveResult { Success = false, Message = "Update for both not implemented." };
            }
        }
        else if (req.IsName)
        {
            // Use OperationResult for name-only
            var opResult = req.NameRecordID == null
                ? banInstanceManager.AddBlacklistNameRecord(
                    req.PlayerName!, req.BanDate, req.ExpireDate, req.RecordType, req.Notes, 0, req.IgnoreValidation)
                : banInstanceManager.UpdateBlacklistNameRecord(
                    req.NameRecordID.Value, req.PlayerName!, req.BanDate, req.ExpireDate, req.RecordType, req.Notes);

            result = new BanRecordSaveResult
            {
                Success = opResult.Success,
                Message = opResult.Message,
                NameRecordID = opResult.RecordID
            };
        }
        else if (req.IsIP)
        {
            // Use OperationResult for IP-only
            var opResult = req.IPRecordID == null
                ? banInstanceManager.AddBlacklistIPRecord(
                    ip!, req.SubnetMask ?? 32, req.BanDate, req.ExpireDate, req.RecordType, req.Notes, 0, req.IgnoreValidation)
                : banInstanceManager.UpdateBlacklistIPRecord(
                    req.IPRecordID.Value, ip!, req.SubnetMask ?? 32, req.BanDate, req.ExpireDate, req.RecordType, req.Notes);

            result = new BanRecordSaveResult
            {
                Success = opResult.Success,
                Message = opResult.Message,
                IPRecordID = opResult.RecordID
            };
        }
        else
        {
            result = new BanRecordSaveResult { Success = false, Message = "No valid data to save." };
        }

        CommonCore.instanceBans!.ForceUIUpdates = true;

        return Ok(result);
    }

    [HttpPost("delete-blacklist")]
    public ActionResult<CommandResult> DeleteBlacklistRecord([FromBody] DeleteBanRecordRequest req)
    {
        if (!HasPermission("bans")) return Forbid();

        if (req == null || req.RecordID <= 0)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        OperationResult opResult;
        if (req.IsName)
            opResult = banInstanceManager.DeleteBlacklistNameRecord(req.RecordID);
        else
            opResult = banInstanceManager.DeleteBlacklistIPRecord(req.RecordID);

        if (!opResult.Success)
            return Ok(new CommandResult { Success = false, Message = opResult.Message });

        CommonCore.instanceBans!.ForceUIUpdates = true;
        return Ok(new CommandResult { Success = true, Message = "Record deleted." });
    }

    [HttpPost("save-whitelist")]
    public ActionResult<BanRecordSaveResult> SaveWhitelistRecord([FromBody] BanDTOs req)
    {
        if (!HasPermission("bans")) return Forbid();

        IPAddress? ip = null;
        if (req.IsIP && !string.IsNullOrWhiteSpace(req.IPAddress))
        {
            if (!IPAddress.TryParse(req.IPAddress, out ip))
                return BadRequest(new BanRecordSaveResult { Success = false, Message = "Invalid IP address." });
        }

        BanRecordSaveResult result;

        if (req.IsName && req.IsIP)
        {
            if (req.NameRecordID == null && req.IPRecordID == null)
            {
                // Use DualRecordResult for both-record creation
                var dualResult = banInstanceManager.AddWhitelistBothRecords(
                    req.PlayerName!, ip!, req.SubnetMask ?? 32, req.BanDate, req.ExpireDate, req.RecordType, req.Notes, req.IgnoreValidation
                );
                result = new BanRecordSaveResult
                {
                    Success = dualResult.Success,
                    Message = dualResult.Message,
                    NameRecordID = dualResult.NameRecordID,
                    IPRecordID = dualResult.IPRecordID
                };
            }
            else
            {
                result = new BanRecordSaveResult { Success = false, Message = "Update for both not implemented." };
            }
        }
        else if (req.IsName)
        {
            // Use OperationResult for name-only
            var opResult = req.NameRecordID == null
                ? banInstanceManager.AddWhitelistNameRecord(
                    req.PlayerName!, req.BanDate, req.ExpireDate, req.RecordType, req.Notes,0,req.IgnoreValidation)
                : banInstanceManager.UpdateWhitelistNameRecord(
                    req.NameRecordID.Value, req.PlayerName!, req.BanDate, req.ExpireDate, req.RecordType, req.Notes);

            result = new BanRecordSaveResult
            {
                Success = opResult.Success,
                Message = opResult.Message,
                NameRecordID = opResult.RecordID
            };
        }
        else if (req.IsIP)
        {
            // Use OperationResult for IP-only
            var opResult = req.IPRecordID == null
                ? banInstanceManager.AddWhitelistIPRecord(
                    ip!, req.SubnetMask ?? 32, req.BanDate, req.ExpireDate, req.RecordType, req.Notes,0,req.IgnoreValidation)
                : banInstanceManager.UpdateWhitelistIPRecord(
                    req.IPRecordID.Value, ip!, req.SubnetMask ?? 32, req.BanDate, req.ExpireDate, req.RecordType, req.Notes);

            result = new BanRecordSaveResult
            {
                Success = opResult.Success,
                Message = opResult.Message,
                IPRecordID = opResult.RecordID
            };
        }
        else
        {
            result = new BanRecordSaveResult { Success = false, Message = "No valid data to save." };
        }

        CommonCore.instanceBans!.ForceUIUpdates = true;

        return Ok(result);
    }

    [HttpPost("delete-whitelist")]
    public ActionResult<CommandResult> DeleteWhitelistRecord([FromBody] DeleteBanRecordRequest req)
    {
        if (!HasPermission("bans")) return Forbid();

        if (req == null || req.RecordID <= 0)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        OperationResult opResult;
        if (req.IsName)
            opResult = banInstanceManager.DeleteWhitelistNameRecord(req.RecordID);
        else
            opResult = banInstanceManager.DeleteWhitelistIPRecord(req.RecordID);

        if (!opResult.Success)
            return Ok(new CommandResult { Success = false, Message = opResult.Message });

        CommonCore.instanceBans!.ForceUIUpdates = true;
        return Ok(new CommandResult { Success = true, Message = "Record deleted." });
    }

    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

}