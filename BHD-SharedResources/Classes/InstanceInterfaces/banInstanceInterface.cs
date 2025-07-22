using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.InstanceInterfaces
{
    public interface banInstanceInterface
    {
        void LoadSettings();
        void SaveSettings();
        void AddBannedPlayer(string playerName, IPAddress playerIP = null!, int subnetMask = 32);
        bool RemoveBannedPlayerName(int recordId);
        bool RemoveBannedPlayerAddress(int recordId);
        bool RemoveBannedPlayerBoth(int recordId);
        void UpdateBannedTables();

    }
}
