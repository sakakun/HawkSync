using BHD_ServerManager.Forms.Panels;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.Instances;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class gamePlayInstanceManager
    {

        /// <summary>
        /// Build GamePlaySettings DTO from theInstance (for UI population)
        /// </summary>
        public static GamePlaySettings BuildGamePlaySettingsFromInstance(theInstance instance)
        {
            var options = new ServerOptions(
                AutoBalance: instance.gameOptionAutoBalance,
                ShowTracers: instance.gameOptionShowTracers,
                ShowClays: instance.gameShowTeamClays,
                AutoRange: instance.gameOptionAutoRange,
                CustomSkins: instance.gameCustomSkins,
                DestroyBuildings: instance.gameDestroyBuildings,
                FatBullets: instance.gameFatBullets,
                OneShotKills: instance.gameOneShotKills,
                AllowLeftLeaning: instance.gameAllowLeftLeaning,
                AllowRightLeaning: instance.gameAllowRightLeaning
            );

            var friendlyFire = new FriendlyFireSettings(
                Enabled: instance.gameOptionFF,
                MaxKills: instance.gameFriendlyFireKills,
                WarnOnKill: instance.gameOptionFFWarn,
                ShowFriendlyTags: instance.gameOptionFriendlyTags
            );

            var roles = new RoleRestrictions(
                CQB: instance.roleCQB,
                Gunner: instance.roleGunner,
                Sniper: instance.roleSniper,
                Medic: instance.roleMedic
            );

            var weapons = new WeaponEnablement(
                Colt45: instance.weaponColt45,
                M9Beretta: instance.weaponM9Beretta,
                CAR15: instance.weaponCar15,
                CAR15203: instance.weaponCar15203,
                M16: instance.weaponM16,
                M16203: instance.weaponM16203,
                G3: instance.weaponG3,
                G36: instance.weaponG36,
                M60: instance.weaponM60,
                M240: instance.weaponM240,
                MP5: instance.weaponMP5,
                SAW: instance.weaponSAW,
                MCRT300: instance.weaponMCRT300,
                M21: instance.weaponM21,
                M24: instance.weaponM24,
                Barrett: instance.weaponBarrett,
                PSG1: instance.weaponPSG1,
                Shotgun: instance.weaponShotgun,
                FragGrenade: instance.weaponFragGrenade,
                SmokeGrenade: instance.weaponSmokeGrenade,
                Satchel: instance.weaponSatchelCharges,
                AT4: instance.weaponAT4,
                FlashBang: instance.weaponFlashGrenade,
                Claymore: instance.weaponClaymore
            );

            return new GamePlaySettings(
                BluePassword: instance.gamePasswordBlue,
                RedPassword: instance.gamePasswordRed,
                ScoreKOTH: instance.gameScoreZoneTime,
                ScoreDM: instance.gameScoreKills,
                ScoreFB: instance.gameScoreFlags,
                TimeLimit: instance.gameTimeLimit,
                LoopMaps: instance.gameLoopMaps,
                StartDelay: instance.gameStartDelay,
                RespawnTime: instance.gameRespawnTime,
                ScoreBoardDelay: instance.gameScoreBoardDelay,
                MaxSlots: instance.gameMaxSlots,
                PSPTakeoverTimer: instance.gamePSPTOTimer,
                FlagReturnTime: instance.gameFlagReturnTime,
                MaxTeamLives: instance.gameMaxTeamLives,
                FullWeaponThreshold: instance.gameFullWeaponThreshold,
                Options: options,
                FriendlyFire: friendlyFire,
                Roles: roles,
                Weapons: weapons,
                RestrictedWeapons: new RestrictedWeapons(
                    instance.restrictedWeaponColt45, instance.restrictedWeaponM9Beretta,
                    instance.restrictedWeaponCar15, instance.restrictedWeaponCar15203,
                    instance.restrictedWeaponM16, instance.restrictedWeaponM16203,
                    instance.restrictedWeaponG3, instance.restrictedWeaponG36,
                    instance.restrictedWeaponM60, instance.restrictedWeaponM240,
                    instance.restrictedWeaponMP5, instance.restrictedWeaponSAW,
                    instance.restrictedWeaponMCRT300, instance.restrictedWeaponM21,
                    instance.restrictedWeaponM24, instance.restrictedWeaponBarrett,
                    instance.restrictedWeaponPSG1, instance.restrictedWeaponShotgun,
                    instance.restrictedWeaponFragGrenade, instance.restrictedWeaponSmokeGrenade,
                    instance.restrictedWeaponSatchelCharges, instance.restrictedWeaponAT4,
                    instance.restrictedWeaponFlashGrenade, instance.restrictedWeaponClaymore
                )
            );
        }
    }
}