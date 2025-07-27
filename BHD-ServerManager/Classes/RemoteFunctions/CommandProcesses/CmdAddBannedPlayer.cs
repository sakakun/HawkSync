using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdAddBannedPlayer")]
    public static class CmdAddBannedPlayer
    {

        public static CommandResponse ProcessCommand(object data)
        {

            string encodedPlayerName = string.Empty;
            string ipAddress = string.Empty;
            int submask = 0;

            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("EncodedPlayerName", out var nameProp))
                    encodedPlayerName = nameProp.GetString() ?? string.Empty;
                if (jsonElement.TryGetProperty("IPAddress", out var ipProp))
                    ipAddress = ipProp.GetString() ?? string.Empty;
                if (jsonElement.TryGetProperty("Submask", out var submaskProp))
                    submask = submaskProp.GetInt32();
            }

            IPAddress? playerIPAddress = null;

            if (!string.IsNullOrWhiteSpace(ipAddress) && IPAddress.TryParse(ipAddress, out var parsedIP))
            {
                playerIPAddress = parsedIP;
            }

            banInstanceManager.AddBannedPlayer(encodedPlayerName, playerIPAddress, submask);

            return new CommandResponse
            {
                Success = true,
                Message = $"Ban command processed.",
                ResponseData = true.ToString()
            };
        }

    }
}
