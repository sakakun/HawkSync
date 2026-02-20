using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabGameplay;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.Instances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GamePlayController : ControllerBase
{
    /// <summary>
    /// Get current gameplay settings
    /// </summary>
    [HttpGet("settings")]
    public ActionResult<GamePlaySettingsResponse> GetSettings()
    {
        if(!HasPermission("gameplay")) return Forbid();

        var result = theInstanceManager.LoadGamePlaySettings();
        
        if (!result.Success)
        {
            return Ok(new GamePlaySettingsResponse
            {
                Success = false,
                Message = result.Message
            });
        }

        // Map from theInstance to DTO
        var settings = MapToSettingsData();

        return Ok(new GamePlaySettingsResponse
        {
            Success = true,
            Message = "Settings loaded successfully",
            Settings = settings
        });
    }

    /// <summary>
    /// Save gameplay settings
    /// </summary>
    [HttpPost("settings")]
    public ActionResult<CommandResult> SaveSettings([FromBody] GamePlaySettingsRequest request)
    {
        if(!HasPermission("gameplay")) return Forbid();

        var options = new ServerOptions(
            request.Options.AutoBalance, request.Options.ShowTracers,
            request.Options.ShowClays, request.Options.AutoRange,
            request.Options.CustomSkins, request.Options.DestroyBuildings,
            request.Options.FatBullets, request.Options.OneShotKills,
            request.Options.AllowLeftLeaning, request.Options.AllowRightLeaning
        );
        var friendlyFire = new FriendlyFireSettings(
            request.FriendlyFire.Enabled, request.FriendlyFire.MaxKills,
            request.FriendlyFire.WarnOnKill, request.FriendlyFire.ShowFriendlyTags
        );
        var roles = new RoleRestrictions(
            request.Roles.CQB, request.Roles.Gunner,
            request.Roles.Sniper, request.Roles.Medic
        );
        var weapons = new WeaponRestrictions(
            request.Weapons.Colt45, request.Weapons.M9Beretta,
            request.Weapons.CAR15, request.Weapons.CAR15203,
            request.Weapons.M16, request.Weapons.M16203,
            request.Weapons.G3, request.Weapons.G36,
            request.Weapons.M60, request.Weapons.M240,
            request.Weapons.MP5, request.Weapons.SAW,
            request.Weapons.MCRT300, request.Weapons.M21,
            request.Weapons.M24, request.Weapons.Barrett,
            request.Weapons.PSG1, request.Weapons.Shotgun,
            request.Weapons.FragGrenade, request.Weapons.SmokeGrenade,
            request.Weapons.Satchel, request.Weapons.AT4,
            request.Weapons.FlashBang, request.Weapons.Claymore
        );
        var limitedWeapons = new LimitedWeaponRestrictions(
            request.LimitedWeapons.Colt45, request.LimitedWeapons.M9Beretta,
            request.LimitedWeapons.CAR15, request.LimitedWeapons.CAR15203,
            request.LimitedWeapons.M16, request.LimitedWeapons.M16203,
            request.LimitedWeapons.G3, request.LimitedWeapons.G36,
            request.LimitedWeapons.M60, request.LimitedWeapons.M240,
            request.LimitedWeapons.MP5, request.LimitedWeapons.SAW,
            request.LimitedWeapons.MCRT300, request.LimitedWeapons.M21,
            request.LimitedWeapons.M24, request.LimitedWeapons.Barrett,
            request.LimitedWeapons.PSG1, request.LimitedWeapons.Shotgun,
            request.LimitedWeapons.FragGrenade, request.LimitedWeapons.SmokeGrenade,
            request.LimitedWeapons.Satchel, request.LimitedWeapons.AT4,
            request.LimitedWeapons.FlashBang, request.LimitedWeapons.Claymore
        );
        var settings = new GamePlaySettings(
            request.BluePassword, request.RedPassword,
            request.ScoreKOTH, request.ScoreDM, request.ScoreFB,
            request.TimeLimit, request.LoopMaps, request.StartDelay,
            request.RespawnTime, request.ScoreBoardDelay, request.MaxSlots,
            request.PSPTakeoverTimer, request.FlagReturnTime, request.MaxTeamLives,
            request.FullWeaponThreshold,
            options, friendlyFire, roles, weapons, limitedWeapons
        );
        var result = theInstanceManager.SaveGamePlaySettings(settings);
        if (result.Success)
        {
            TriggerServerUIReload();
        }
        LogGamePlayAction("SaveSettings", "Gameplay settings updated", result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    /// <summary>
    /// Validate gameplay settings without saving
    /// </summary>
    [HttpPost("validate")]
    public ActionResult<ValidationResult> ValidateSettings([FromBody] GamePlaySettingsRequest request)
    {
        if(!HasPermission("gameplay")) return Forbid();

        // For gameplay, basic validation can be done here
        var errors = new List<string>();

        // Validate numeric ranges
        if (request.ScoreKOTH < 0 || request.ScoreKOTH > 9999)
            errors.Add("KOTH score must be between 0 and 9999.");

        if (request.ScoreDM < 0 || request.ScoreDM > 9999)
            errors.Add("Deathmatch score must be between 0 and 9999.");

        if (request.ScoreFB < 0 || request.ScoreFB > 9999)
            errors.Add("Flag score must be between 0 and 9999.");

        if (request.TimeLimit < 0 || request.TimeLimit > 999)
            errors.Add("Time limit must be between 0 and 999 minutes.");

        if (request.MaxSlots < 1 || request.MaxSlots > 50)
            errors.Add("Max players must be between 1 and 50.");

        if (request.RespawnTime < 0 || request.RespawnTime > 999)
            errors.Add("Respawn time must be between 0 and 999 seconds.");

        return Ok(new ValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        });
    }

    // ================================================================================
    // HELPER METHODS
    // ================================================================================
    private void LogGamePlayAction(string actionType, string description, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.Settings, // or AuditCategory.Gameplay if you have one
            actionType: actionType,
            description: description,
            targetType: "GamePlay",
            targetId: null,
            targetName: null,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

    private GamePlaySettingsData MapToSettingsData()
    {
        var instance = CommonCore.theInstance!;
        
        var options = new ServerOptionsDTO(
            instance.gameOptionAutoBalance,
            instance.gameOptionShowTracers,
            instance.gameShowTeamClays,
            instance.gameOptionAutoRange,
            instance.gameCustomSkins,
            instance.gameDestroyBuildings,
            instance.gameFatBullets,
            instance.gameOneShotKills,
            instance.gameAllowLeftLeaning,
            instance.gameAllowRightLeaning
        );

        var friendlyFire = new FriendlyFireSettingsDTO(
            instance.gameOptionFF,
            instance.gameFriendlyFireKills,
            instance.gameOptionFFWarn,
            instance.gameOptionFriendlyTags
        );

        var roles = new RoleRestrictionsDTO(
            instance.roleCQB,
            instance.roleGunner,
            instance.roleSniper,
            instance.roleMedic
        );


        var weapons = new WeaponRestrictionsDTO(
            instance.weaponColt45, instance.weaponM9Beretta,
            instance.weaponCar15, instance.weaponCar15203,
            instance.weaponM16, instance.weaponM16203,
            instance.weaponG3, instance.weaponG36,
            instance.weaponM60, instance.weaponM240,
            instance.weaponMP5, instance.weaponSAW,
            instance.weaponMCRT300, instance.weaponM21,
            instance.weaponM24, instance.weaponBarrett,
            instance.weaponPSG1, instance.weaponShotgun,
            instance.weaponFragGrenade, instance.weaponSmokeGrenade,
            instance.weaponSatchelCharges, instance.weaponAT4,
            instance.weaponFlashGrenade, instance.weaponClaymore
        );

        var limitedWeapons = new LimitedWeaponRestrictionsDTO(
            instance.limitedWeaponColt45, instance.limitedWeaponM9Beretta,
            instance.limitedWeaponCar15, instance.limitedWeaponCar15203,
            instance.limitedWeaponM16, instance.limitedWeaponM16203,
            instance.limitedWeaponG3, instance.limitedWeaponG36,
            instance.limitedWeaponM60, instance.limitedWeaponM240,
            instance.limitedWeaponMP5, instance.limitedWeaponSAW,
            instance.limitedWeaponMCRT300, instance.limitedWeaponM21,
            instance.limitedWeaponM24, instance.limitedWeaponBarrett,
            instance.limitedWeaponPSG1, instance.limitedWeaponShotgun,
            instance.limitedWeaponFragGrenade, instance.limitedWeaponSmokeGrenade,
            instance.limitedWeaponSatchelCharges, instance.limitedWeaponAT4,
            instance.limitedWeaponFlashGrenade, instance.limitedWeaponClaymore
        );

        return new GamePlaySettingsData
        {
            BluePassword = instance.gamePasswordBlue,
            RedPassword = instance.gamePasswordRed,
            ScoreKOTH = instance.gameScoreZoneTime,
            ScoreDM = instance.gameScoreKills,
            ScoreFB = instance.gameScoreFlags,
            TimeLimit = instance.gameTimeLimit,
            LoopMaps = instance.gameLoopMaps,
            StartDelay = instance.gameStartDelay,
            RespawnTime = instance.gameRespawnTime,
            ScoreBoardDelay = instance.gameScoreBoardDelay,
            MaxSlots = instance.gameMaxSlots,
            PSPTakeoverTimer = instance.gamePSPTOTimer,
            FlagReturnTime = instance.gameFlagReturnTime,
            MaxTeamLives = instance.gameMaxTeamLives,
            FullWeaponThreshold = instance.gameFullWeaponThreshold,
            Options = options,
            FriendlyFire = friendlyFire,
            Roles = roles,
            Weapons = weapons,
            LimitedWeapons = limitedWeapons
        };
    }

    private void TriggerServerUIReload()
    {
        // Trigger UI update on server manager via invoke
        var serverUI = Program.ServerManagerUI;
        if (serverUI != null)
        {
            serverUI.Invoke(() =>
            {
                serverUI.GamePlayTab?.LoadSettings();
            });
        }
    }

    /// <summary>
    /// Update running game server with current gameplay settings
    /// </summary>
    [HttpPost("update-server")]
    public ActionResult<CommandResult> UpdateGameServer()
    {
        if(!HasPermission("gameplay")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            if (CommonCore.theInstance!.instanceStatus == InstanceStatus.OFFLINE)
            {
                message = "Game server is not running. Cannot update settings.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
            theInstanceManager.UpdateGameServer();
            success = true;
            message = "Gameplay settings have been applied to the running game server.";
            return Ok(new CommandResult { Success = success, Message = message });
        }
        catch (Exception ex)
        {
            message = $"Error updating game server: {ex.Message}";
            return Ok(new CommandResult { Success = false, Message = message });
        }
        finally
        {
            LogGamePlayAction("UpdateGameServer", "Applied gameplay settings to running server", success, message);
        }
    }

    /// <summary>
    /// Lock the game lobby (prevent new players from joining)
    /// </summary>
    [HttpPost("lock-lobby")]
    public ActionResult<CommandResult> LockLobby()
    {
        if(!HasPermission("gameplay")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            if (CommonCore.theInstance!.instanceStatus == InstanceStatus.OFFLINE)
            {
                message = "Game server is not running. Cannot un/lock lobby.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
            ServerMemory.WriteMemorySendConsoleCommand("lockgame");
            success = true;
            message = "Game lobby has been un/locked.";
            return Ok(new CommandResult { Success = success, Message = message });
        }
        catch (Exception ex)
        {
            message = $"Error un/locking lobby: {ex.Message}";
            return Ok(new CommandResult { Success = false, Message = message });
        }
        finally
        {
            LogGamePlayAction("LockLobby", "Game lobby lock/unlock", success, message);
        }
    }

    /// <summary>
    /// Export current gameplay settings as JSON
    /// </summary>
    [HttpGet("export")]
    public ActionResult<GamePlaySettingsExportResponse> ExportSettings()
    {
        if(!HasPermission("gameplay")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            var result = theInstanceManager.LoadGamePlaySettings();
            if (!result.Success)
            {
                message = result.Message;
                return Ok(new GamePlaySettingsExportResponse { Success = false, Message = message });
            }
            var instance = CommonCore.theInstance!;
            var options = new ServerOptions(
                instance.gameOptionAutoBalance, instance.gameOptionShowTracers,
                instance.gameShowTeamClays, instance.gameOptionAutoRange,
                instance.gameCustomSkins, instance.gameDestroyBuildings,
                instance.gameFatBullets, instance.gameOneShotKills,
                instance.gameAllowLeftLeaning, instance.gameAllowRightLeaning
            );
            var friendlyFire = new FriendlyFireSettings(
                instance.gameOptionFF, instance.gameFriendlyFireKills,
                instance.gameOptionFFWarn, instance.gameOptionFriendlyTags
            );
            var roles = new RoleRestrictions(
                instance.roleCQB, instance.roleGunner,
                instance.roleSniper, instance.roleMedic
            );
            var weapons = new WeaponRestrictions(
                instance.weaponColt45, instance.weaponM9Beretta,
                instance.weaponCar15, instance.weaponCar15203,
                instance.weaponM16, instance.weaponM16203,
                instance.weaponG3, instance.weaponG36,
                instance.weaponM60, instance.weaponM240,
                instance.weaponMP5, instance.weaponSAW,
                instance.weaponMCRT300, instance.weaponM21,
                instance.weaponM24, instance.weaponBarrett,
                instance.weaponPSG1, instance.weaponShotgun,
                instance.weaponFragGrenade, instance.weaponSmokeGrenade,
                instance.weaponSatchelCharges, instance.weaponAT4,
                instance.weaponFlashGrenade, instance.weaponClaymore
            );
            var limitedWeapons = new LimitedWeaponRestrictions(
                instance.limitedWeaponColt45, instance.limitedWeaponM9Beretta,
                instance.limitedWeaponCar15, instance.limitedWeaponCar15203,
                instance.limitedWeaponM16, instance.limitedWeaponM16203,
                instance.limitedWeaponG3, instance.limitedWeaponG36,
                instance.limitedWeaponM60, instance.limitedWeaponM240,
                instance.limitedWeaponMP5, instance.limitedWeaponSAW,
                instance.limitedWeaponMCRT300, instance.limitedWeaponM21,
                instance.limitedWeaponM24, instance.limitedWeaponBarrett,
                instance.limitedWeaponPSG1, instance.limitedWeaponShotgun,
                instance.limitedWeaponFragGrenade, instance.limitedWeaponSmokeGrenade,
                instance.limitedWeaponSatchelCharges, instance.limitedWeaponAT4,
                instance.limitedWeaponFlashGrenade, instance.limitedWeaponClaymore
            );
            var settings = new GamePlaySettings(
                instance.gamePasswordBlue, instance.gamePasswordRed,
                instance.gameScoreZoneTime, instance.gameScoreKills, instance.gameScoreFlags,
                instance.gameTimeLimit, instance.gameLoopMaps, instance.gameStartDelay,
                instance.gameRespawnTime, instance.gameScoreBoardDelay, instance.gameMaxSlots,
                instance.gamePSPTOTimer, instance.gameFlagReturnTime, instance.gameMaxTeamLives,
                instance.gameFullWeaponThreshold,
                options, friendlyFire, roles, weapons, limitedWeapons
            );
            var json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            success = true;
            message = "Settings exported successfully";
            return Ok(new GamePlaySettingsExportResponse
            {
                Success = true,
                Message = message,
                JsonData = json,
                FileName = $"GamePlaySettings_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            });
        }
        catch (Exception ex)
        {
            message = $"Error exporting settings: {ex.Message}";
            return Ok(new GamePlaySettingsExportResponse { Success = false, Message = message });
        }
        finally
        {
            LogGamePlayAction("ExportSettings", "Exported gameplay settings as JSON", success, message);
        }
    }

    /// <summary>
    /// Import gameplay settings from JSON data
    /// </summary>
    [HttpPost("import")]
    public ActionResult<CommandResult> ImportSettings([FromBody] GamePlaySettingsImportRequest request)
    {
        if(!HasPermission("gameplay")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            var settings = System.Text.Json.JsonSerializer.Deserialize<GamePlaySettings>(request.JsonData);
            if (settings == null)
            {
                message = "Failed to parse JSON data. Invalid format.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
            var result = theInstanceManager.SaveGamePlaySettings(settings);
            if (result.Success)
            {
                TriggerServerUIReload();
            }
            success = result.Success;
            message = result.Success 
                ? "Settings imported and applied successfully." 
                : $"Failed to import settings: {result.Message}";
            return Ok(new CommandResult { Success = success, Message = message });
        }
        catch (System.Text.Json.JsonException ex)
        {
            message = $"Invalid JSON format: {ex.Message}";
            return Ok(new CommandResult { Success = false, Message = message });
        }
        catch (Exception ex)
        {
            message = $"Error importing settings: {ex.Message}";
            return Ok(new CommandResult { Success = false, Message = message });
        }
        finally
        {
            LogGamePlayAction("ImportSettings", "Imported gameplay settings from JSON", success, message);
        }
    }

    /// <summary>
    /// Start the game server
    /// </summary>
    [HttpPost("start-server")]
    public ActionResult<CommandResult> StartServer()
    {
        if(!HasPermission("gameplay")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            var instance = CommonCore.theInstance;
            if (instance != null && instance.instanceStatus != InstanceStatus.OFFLINE)
            {
                message = "Server is already running.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
            if (!theInstanceManager.ValidateGameServerPath())
            {
                message = "Invalid game server path. Please configure the server path in Profile settings.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
            bool started = BHD_ServerManager.Classes.GameManagement.StartServer.startGame();
            if (started)
            {
                ServerMemory.ReadMemoryServerStatus();
                success = true;
                message = "Game server started successfully.";
                return Ok(new CommandResult { Success = success, Message = message });
            }
            else
            {
                message = "Failed to start game server. Check server logs for details.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
        }
        catch (Exception ex)
        {
            message = $"Error starting server: {ex.Message}";
            return Ok(new CommandResult { Success = false, Message = message });
        }
        finally
        {
            LogGamePlayAction("StartServer", "Started game server", success, message);
        }
    }

    /// <summary>
    /// Stop the game server
    /// </summary>
    [HttpPost("stop-server")]
    public ActionResult<CommandResult> StopServer()
    {
        if(!HasPermission("gameplay")) return Forbid();

        bool success = false;
        string message = string.Empty;
        try
        {
            var instance = CommonCore.theInstance;
            if (instance == null || instance.instanceStatus == InstanceStatus.OFFLINE)
            {
                message = "Server is not running.";
                return Ok(new CommandResult { Success = false, Message = message });
            }
            BHD_ServerManager.Classes.GameManagement.StartServer.stopGame();
            success = true;
            message = "Game server stopped successfully.";
            return Ok(new CommandResult { Success = success, Message = message });
        }
        catch (Exception ex)
        {
            message = $"Error stopping server: {ex.Message}";
            return Ok(new CommandResult { Success = false, Message = message });
        }
        finally
        {
            LogGamePlayAction("StopServer", "Stopped game server", success, message);
        }
    }
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }
}