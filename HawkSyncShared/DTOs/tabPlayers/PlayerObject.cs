using System;
using System.Collections.Generic;

namespace HawkSyncShared.DTOs.tabPlayers
{
    public class PlayerObject
    {
        // Player Specific Information
        public int PlayerSlot { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string PlayerNameBase64 { get; set; } = string.Empty;
        public string PlayerIPAddress { get; set; } = string.Empty;
        public int PlayerTeam { get; set; } // 0=Self, 1= Blue, 2 Red PlayerTeam

        // Player Status Information
        public int PlayerPing { get; set; } = 0;
        public int PlayerTimePlayed { get; set; } = 0; // Total time played in seconds
        public DateTime PlayerJoined { get; set; } = DateTime.Now;
        public DateTime PlayerLastSeen { get; set; } = DateTime.Now;

        // God Mode
        public bool IsGod { get; set; } = false;
        public bool IsProxyDetected { get; set; } = false;
        public string? CountryCode { get; set; }

        // Character Information
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // In-Game Values
        public int ActiveZoneTime { get; set; }
        public int PlayerHealth { get; set; } = 0;

        // In-Game Position and Facing Information
        public float PosX { get; set; } = 0f;
        public float PosY { get; set; } = 0f;
        public float PosZ { get; set; } = 0f;
        public float FacingYaw { get; set; } = 0f;
        public float FacingPitch { get; set; } = 0f;

        // Player Statistics
        public int stat_Kills { get; set; }
        public int stat_Deaths { get; set; }
        public int stat_ZoneTime { get; set; }
        public int stat_ZoneKills { get; set; }
        public int stat_ZoneDefendKills { get; set; }
        public int stat_RevivesGiven { get; set; }
        public int stat_FBCaptures { get; set; }
        public int stat_Suicides { get; set; }
        public int stat_Murders { get; set; }
        public int stat_Headshots { get; set; }
        public int stat_KnifeKills { get; set; }
        public int stat_RevivesReceived { get; set; }
        public int stat_PSPAttempts { get; set; }
        public int stat_PSPTakeovers { get; set; }
        public int stat_DoubleKills { get; set; }
        public int stat_FBCarrierKills { get; set; }
        public int stat_FBCarrierDeaths { get; set; }
        public int stat_ExperiencePoints { get; set; }
        public int stat_SDADTargetsDestroyed { get; set; }
        public int stat_SDADDefenseKills { get; set; }
        public int stat_SDADAttackKills { get; set; }
        public int stat_FlagSaves { get; set; }
        public int stat_SniperKills { get; set; }
        public int stat_TKOTHDefenseKills { get; set; }
        public int stat_TKOTHAttackKills { get; set; }
        public int stat_TotalShotsFired { get; set; }

        // Player Weapon Information
        public List<string> PlayerWeapons { get; set; } = new List<string>();
        public int SelectedWeaponID { get; set; }
        public string SelectedWeaponName { get; set; } = string.Empty;

    }
    public class playerTeamObject
    {
        public int slotNum { get; set; }
        public int Team { get; set; }
    }

    public enum WeaponStack
    {
        WPN_KNIFE = 1, // default weapon

        // secondary PlayerSlot
        WPN_colt45 = 2,
        WPN_M9Beretta = 3,
        WPN_RemmingtonSG = 4,

        // primary PlayerSlot
        WPN_CAR15_AUTO = 5,
        WPN_CAR15_SEMI = 6,
        WPN_CAR15_203_AUTO = 7,
        WPN_CAR15_203_SEMI = 8,
        WPN_CAR15_203_203 = 9,
        WPN_M16_Burst = 10,
        WPN_M16_SEMI = 11,
        WPN_M16_203_Burst = 12,
        WPN_M16_203_SEMI = 13,
        WPN_M16_203_203 = 14,
        WPN_M21 = 15,
        WPN_M24 = 16,
        WPN_MCRT_300_TACTICAL = 17,
        WPN_Barrett = 18,
        WPN_SAW = 19,
        WPN_M60 = 20,
        WPN_M240 = 21,
        WPN_MP5 = 22,

        // Team Sabre Expanssion Weapons
        WPN_G3_Auto = 23,
        WPN_G3_SEMI = 24,
        WPN_G36_AUTO = 25,
        WPN_G36_SEMI = 26,
        WPN_PSG1 = 27,

        // grenades
        WPN_XM84_STUN = 28,
        WPN_M67_FRAG = 29,
        WPN_AN_M8_SMOKE = 30,
        WPN_Satchel_CHARGE = 32,
        WPN_Radio_DETONATOR = 33,
        WPN_Claymore = 34,
        WPN_AT4 = 35,


        // medkit
        WPN_MEDPACK = 31,

        // mounts
        WPN_50_Hummer = 36,
        WPN_Mini_GUN = 37,
        WPN_GRENADELAUNCHER_M203 = 38,
        WPN_50_EMPLACEMENT_TRUCK = 40,
        WPN_50_EMPLACEMENT = 41,
        WPN_EMC_CANNON = 42,
    }
    public enum Teams
    {
        TEAM_GREEN = 0,   // Deathmatch/FFA (non-team modes)
        TEAM_BLUE = 1,    // 2-team & 4-team modes
        TEAM_RED = 2,     // 2-team & 4-team modes
        TEAM_YELLOW = 3,  // 4-team mode only
        TEAM_PURPLE = 4,  // 4-team mode only
        TEAM_SPEC = 5     // Not used
    }
    public enum CharacterClass
    {
        CQB = 5,
        MEDIC = 6,
        SNIPER = 7,
        GUNNER = 8,
        SAS_CQB = 9,
        SAS_MEDIC = 10,
        SAS_SNIPER = 11,
        SAS_GUNNER = 12,
    }

    public record PlayerIdleState
    {
        public float PosX, PosY, PosZ;
        public float FacingYaw, FacingPitch;
        public int ShotsFired;
        public int Health;
        public DateTime LastActive;
    }

    public static class PlayerObjectSanitizer
    {
        public static PlayerObject Sanitize(PlayerObject player)
        {
            player.PosX = SanitizeFloat(player.PosX);
            player.PosY = SanitizeFloat(player.PosY);
            player.PosZ = SanitizeFloat(player.PosZ);
            player.FacingYaw = SanitizeFloat(player.FacingYaw);
            player.FacingPitch = SanitizeFloat(player.FacingPitch);
            
            return player;
        }

        private static float SanitizeFloat(float value)
        {
            return (float.IsNaN(value) || float.IsInfinity(value)) ? 0f : value;
        }
    }

}
