using BHD_ServerManager.Forms.Panels;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.Instances;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class gamePlayInstanceManager
    {
        /// <summary>
        /// Build GamePlaySettings DTO from UI controls
        /// </summary>
        public static GamePlaySettings BuildGamePlaySettings(tabGamePlay gamePlayTab)
        {
            var options = new ServerOptions(
                AutoBalance: gamePlayTab.cb_autoBalance.Checked,
                ShowTracers: gamePlayTab.cb_showTracers.Checked,
                ShowClays: gamePlayTab.cb_showClays.Checked,
                AutoRange: gamePlayTab.cb_autoRange.Checked,
                CustomSkins: gamePlayTab.cb_customSkins.Checked,
                DestroyBuildings: gamePlayTab.cb_enableDistroyBuildings.Checked,
                FatBullets: gamePlayTab.cb_enableFatBullets.Checked,
                OneShotKills: gamePlayTab.cb_enableOneShotKills.Checked,
                AllowLeftLeaning: gamePlayTab.cb_enableLeftLean.Checked
            );

            var friendlyFire = new FriendlyFireSettings(
                Enabled: gamePlayTab.cb_enableFFkills.Checked,
                MaxKills: (int)gamePlayTab.num_maxFFKills.Value,
                WarnOnKill: gamePlayTab.cb_warnFFkils.Checked,
                ShowFriendlyTags: gamePlayTab.cb_showTeamTags.Checked
            );

            var roles = new RoleRestrictions(
                CQB: gamePlayTab.cb_roleCQB.Checked,
                Gunner: gamePlayTab.cb_roleGunner.Checked,
                Sniper: gamePlayTab.cb_roleSniper.Checked,
                Medic: gamePlayTab.cb_roleMedic.Checked
            );

            var weapons = new WeaponRestrictions(
                Colt45: gamePlayTab.cb_weapColt45.Checked,
                M9Beretta: gamePlayTab.cb_weapM9Bereatta.Checked,
                CAR15: gamePlayTab.cb_weapCAR15.Checked,
                CAR15203: gamePlayTab.cb_weapCAR15203.Checked,
                M16: gamePlayTab.cb_weapM16.Checked,
                M16203: gamePlayTab.cb_weapM16203.Checked,
                G3: gamePlayTab.cb_weapG3.Checked,
                G36: gamePlayTab.cb_weapG36.Checked,
                M60: gamePlayTab.cb_weapM60.Checked,
                M240: gamePlayTab.cb_weapM240.Checked,
                MP5: gamePlayTab.cb_weapMP5.Checked,
                SAW: gamePlayTab.cb_weapSaw.Checked,
                MCRT300: gamePlayTab.cb_weap300Tact.Checked,
                M21: gamePlayTab.cb_weapM21.Checked,
                M24: gamePlayTab.cb_weapM24.Checked,
                Barrett: gamePlayTab.cb_weapBarret.Checked,
                PSG1: gamePlayTab.cb_weapPSG1.Checked,
                Shotgun: gamePlayTab.cb_weapShotgun.Checked,
                FragGrenade: gamePlayTab.cb_weapFragGrenade.Checked,
                SmokeGrenade: gamePlayTab.cb_weapSmokeGrenade.Checked,
                Satchel: gamePlayTab.cb_weapSatchel.Checked,
                AT4: gamePlayTab.cb_weapAT4.Checked,
                FlashBang: gamePlayTab.cb_weapFlashBang.Checked,
                Claymore: gamePlayTab.cb_weapClay.Checked
            );

            return new GamePlaySettings(
                BluePassword: gamePlayTab.tb_bluePassword.Text,
                RedPassword: gamePlayTab.tb_redPassword.Text,
                ScoreKOTH: (int)gamePlayTab.num_scoresKOTH.Value,
                ScoreDM: (int)gamePlayTab.num_scoresDM.Value,
                ScoreFB: (int)gamePlayTab.num_scoresFB.Value,
                TimeLimit: (int)gamePlayTab.num_gameTimeLimit.Value,
                LoopMaps: gamePlayTab.cb_replayMaps.SelectedIndex,
                StartDelay: (int)gamePlayTab.num_gameStartDelay.Value,
                RespawnTime: (int)gamePlayTab.num_respawnTime.Value,
                ScoreBoardDelay: (int)gamePlayTab.num_scoreBoardDelay.Value,
                MaxSlots: (int)gamePlayTab.num_maxPlayers.Value,
                PSPTakeoverTimer: (int)gamePlayTab.num_pspTakeoverTimer.Value,
                FlagReturnTime: (int)gamePlayTab.num_flagReturnTime.Value,
                MaxTeamLives: (int)gamePlayTab.num_maxTeamLives.Value,
                Options: options,
                FriendlyFire: friendlyFire,
                Roles: roles,
                Weapons: weapons
            );
        }

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
                AllowLeftLeaning: instance.gameAllowLeftLeaning
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

            var weapons = new WeaponRestrictions(
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
                Options: options,
                FriendlyFire: friendlyFire,
                Roles: roles,
                Weapons: weapons
            );
        }
    }
}