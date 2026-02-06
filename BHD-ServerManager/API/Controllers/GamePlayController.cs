using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
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

        // Convert DTO to GamePlaySettings
        var options = new ServerOptions(
            request.Options.AutoBalance, request.Options.ShowTracers,
            request.Options.ShowClays, request.Options.AutoRange,
            request.Options.CustomSkins, request.Options.DestroyBuildings,
            request.Options.FatBullets, request.Options.OneShotKills,
            request.Options.AllowLeftLeaning
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

        var settings = new GamePlaySettings(
            request.BluePassword, request.RedPassword,
            request.ScoreKOTH, request.ScoreDM, request.ScoreFB,
            request.TimeLimit, request.LoopMaps, request.StartDelay,
            request.RespawnTime, request.ScoreBoardDelay, request.MaxSlots,
            request.PSPTakeoverTimer, request.FlagReturnTime, request.MaxTeamLives,
            options, friendlyFire, roles, weapons
        );

        // Save via manager (includes validation)
        var result = theInstanceManager.SaveGamePlaySettings(settings);

        if (result.Success)
        {
            // Trigger UI reload on server manager
            TriggerServerUIReload();
        }

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
            instance.gameAllowLeftLeaning
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
            Options = options,
            FriendlyFire = friendlyFire,
            Roles = roles,
            Weapons = weapons
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
                serverUI.ServerTab?.methodFunction_loadSettings();
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

        try
        {
            // Check if server is running
            if (!ServerMemory.ReadMemoryIsProcessAttached())
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Game server is not running. Cannot update settings."
                });
            }

            // Update the game server with current settings from instance
            theInstanceManager.UpdateGameServer();

            return Ok(new CommandResult
            {
                Success = true,
                Message = "Gameplay settings have been applied to the running game server."
            });
        }
        catch (Exception ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Error updating game server: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Lock the game lobby (prevent new players from joining)
    /// </summary>
    [HttpPost("lock-lobby")]
    public ActionResult<CommandResult> LockLobby()
    {
        if(!HasPermission("gameplay")) return Forbid();

        try
        {
            // Check if server is running
            if (!ServerMemory.ReadMemoryIsProcessAttached())
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Game server is not running. Cannot un/lock lobby."
                });
            }

            // Send lock game command to server
            ServerMemory.WriteMemorySendConsoleCommand("lockgame");

            return Ok(new CommandResult
            {
                Success = true,
                Message = "Game lobby has been un/locked."
            });
        }
        catch (Exception ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Error un/locking lobby: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Export current gameplay settings as JSON
    /// </summary>
    [HttpGet("export")]
    public ActionResult<GamePlaySettingsExportResponse> ExportSettings()
    {
        if(!HasPermission("gameplay")) return Forbid();

        try
        {
            var result = theInstanceManager.LoadGamePlaySettings();
        
            if (!result.Success)
            {
                return Ok(new GamePlaySettingsExportResponse
                {
                    Success = false,
                    Message = result.Message
                });
            }

            // Get current settings from instance
            var instance = CommonCore.theInstance!;
        
            var options = new ServerOptions(
                instance.gameOptionAutoBalance, instance.gameOptionShowTracers,
                instance.gameShowTeamClays, instance.gameOptionAutoRange,
                instance.gameCustomSkins, instance.gameDestroyBuildings,
                instance.gameFatBullets, instance.gameOneShotKills,
                instance.gameAllowLeftLeaning
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

            var settings = new GamePlaySettings(
                instance.gamePasswordBlue, instance.gamePasswordRed,
                instance.gameScoreZoneTime, instance.gameScoreKills, instance.gameScoreFlags,
                instance.gameTimeLimit, instance.gameLoopMaps, instance.gameStartDelay,
                instance.gameRespawnTime, instance.gameScoreBoardDelay, instance.gameMaxSlots,
                instance.gamePSPTOTimer, instance.gameFlagReturnTime, instance.gameMaxTeamLives,
                options, friendlyFire, roles, weapons
            );

            // Serialize to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            return Ok(new GamePlaySettingsExportResponse
            {
                Success = true,
                Message = "Settings exported successfully",
                JsonData = json,
                FileName = $"GamePlaySettings_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            });
        }
        catch (Exception ex)
        {
            return Ok(new GamePlaySettingsExportResponse
            {
                Success = false,
                Message = $"Error exporting settings: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Import gameplay settings from JSON data
    /// </summary>
    [HttpPost("import")]
    public ActionResult<CommandResult> ImportSettings([FromBody] GamePlaySettingsImportRequest request)
    {
        if(!HasPermission("gameplay")) return Forbid();

        try
        {
            // Deserialize JSON
            var settings = System.Text.Json.JsonSerializer.Deserialize<GamePlaySettings>(request.JsonData);
        
            if (settings == null)
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Failed to parse JSON data. Invalid format."
                });
            }

            // Save the imported settings
            var result = theInstanceManager.SaveGamePlaySettings(settings);

            if (result.Success)
            {
                // Trigger UI reload on server manager
                TriggerServerUIReload();
            }

            return Ok(new CommandResult
            {
                Success = result.Success,
                Message = result.Success 
                    ? "Settings imported and applied successfully." 
                    : $"Failed to import settings: {result.Message}"
            });
        }
        catch (System.Text.Json.JsonException ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Invalid JSON format: {ex.Message}"
            });
        }
        catch (Exception ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Error importing settings: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Start the game server
    /// </summary>
    [HttpPost("start-server")]
    public ActionResult<CommandResult> StartServer()
    {
        if(!HasPermission("gameplay")) return Forbid();

        try
        {
            var instance = CommonCore.theInstance;
        
            // Check if already running
            if (instance != null && instance.instanceStatus != InstanceStatus.OFFLINE)
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Server is already running."
                });
            }

            // Validate server path
            if (!theInstanceManager.ValidateGameServerPath())
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Invalid game server path. Please configure the server path in Profile settings."
                });
            }

            // Start the server
            bool started = BHD_ServerManager.Classes.GameManagement.StartServer.startGame();

            if (started)
            {
                // Update server status
                ServerMemory.ReadMemoryServerStatus();

                // Trigger UI update on server manager
                var serverUI = Program.ServerManagerUI;
                if (serverUI != null)
                {
                    serverUI.Invoke(() =>
                    {
                        serverUI.ServerTab?.functionEvent_UpdateServerControls();
                        serverUI.MapsTab?.methodFunction_UpdateMapControls();
                    });
                }

                return Ok(new CommandResult
                {
                    Success = true,
                    Message = "Game server started successfully."
                });
            }
            else
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Failed to start game server. Check server logs for details."
                });
            }
        }
        catch (Exception ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Error starting server: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Stop the game server
    /// </summary>
    [HttpPost("stop-server")]
    public ActionResult<CommandResult> StopServer()
    {
        if(!HasPermission("gameplay")) return Forbid();

        try
        {
            var instance = CommonCore.theInstance;
        
            // Check if server is running
            if (instance == null || instance.instanceStatus == InstanceStatus.OFFLINE)
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Server is not running."
                });
            }

            // Stop the server
            BHD_ServerManager.Classes.GameManagement.StartServer.stopGame();

            // Trigger UI update on server manager
            var serverUI = Program.ServerManagerUI;
            if (serverUI != null)
            {
                serverUI.Invoke(() =>
                {
                    serverUI.ServerTab?.functionEvent_UpdateServerControls();
                });
            }

            return Ok(new CommandResult
            {
                Success = true,
                Message = "Game server stopped successfully."
            });
        }
        catch (Exception ex)
        {
            return Ok(new CommandResult
            {
                Success = false,
                Message = $"Error stopping server: {ex.Message}"
            });
        }
    }
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }
}