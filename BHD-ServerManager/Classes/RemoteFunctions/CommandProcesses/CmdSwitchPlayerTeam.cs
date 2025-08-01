using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{

    [CommandHandler("CmdSwitchPlayerTeam")]
    public static class CmdSwitchPlayerTeam
    {
        private static theInstance thisInstance = CommonCore.theInstance;

        public static CommandResponse ProcessCommand(object data)
        {
            int playerSlot = -1;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("playerSlot", out var object1))
                    playerSlot = object1.GetInt32();

            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("playerSlot", out var channelObj) && channelObj is int ch)
                    playerSlot = ch;
            }

            // Check if player is already in the change PlayerTeam list
            var existing = thisInstance.playerChangeTeamList
                .FirstOrDefault(p => p.slotNum == playerSlot);

            var Player = thisInstance.playerList.TryGetValue(playerSlot, out var player) ? player : null;

            if (Player == null)
            {
                return new CommandResponse
                {
                    Success = false,
                    Message = $"Player not found, has the player left the server?",
                    ResponseData = false.ToString()
                };
            }

            if (existing != null)
            {
                // Undo: remove from list
                thisInstance.playerChangeTeamList.Remove(existing);
                string message = $"Team switch for {Player.PlayerName} has been undone.";
                return new CommandResponse
                {
                    Success = true,
                    Message = message,
                    ResponseData = true.ToString()
                };
            }
            else
            {
                int newTeam = Player!.PlayerTeam == 1 ? 2 : Player.PlayerTeam == 2 ? 1 : Player.PlayerTeam;
                if (newTeam != Player.PlayerTeam)
                {
                    thisInstance.playerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerSlot,
                        Team = newTeam
                    });
                    string message = $"Player {Player.PlayerName} has been switched to PlayerTeam {(newTeam == 1 ? "Blue" : "Red")} for the next map.";
                    return new CommandResponse
                    {
                        Success = true,
                        Message = message,
                        ResponseData = true.ToString()
                    };
                }
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Something went wrong, contact the server admin.",
                ResponseData = false.ToString()
            };
        }

    }
}
