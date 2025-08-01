using System;
using System.Text;
using System.Windows.Forms;

namespace BHD_SharedResources.Classes.SupportClasses
{
    public static class Functions
    {
        public static string SanitizePlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            var sb = new StringBuilder(name.Length);
            foreach (char c in name)
            {
                int code = c;
                // Allow printable ASCII (0x20–0x7E) and printable Windows-1251 (0xA0–0xFF)
                if (code >= 0x20 && code <= 0x7E || code >= 0xA0 && code <= 0xFF)
                {
                    sb.Append(c);
                }
                // Optionally, allow space (0x20)
                // else if (code == 0x20) sb.Append(c);
                // Otherwise, skip (removes control chars, newlines, etc.)
            }
            return sb.ToString().Trim();
        }
        public static byte[] ToByteArray(string hexString)
        {
            byte[] retval = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return retval;
        }
        public static string ShowFileDialog(bool saveDialog, string filter, string title, string initialDirectory, string defaultFileName = "")
        {
            if (saveDialog)
            {
                using var sfd = new SaveFileDialog
                {
                    Filter = filter,
                    Title = title,
                    InitialDirectory = initialDirectory,
                    FileName = defaultFileName
                };
                return sfd.ShowDialog() == DialogResult.OK ? sfd.FileName : string.Empty;
            }
            else
            {
                using var ofd = new OpenFileDialog
                {
                    Filter = filter,
                    Title = title,
                    InitialDirectory = initialDirectory
                };
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : string.Empty;
            }
        }
        public static int CalulateGameOptions(bool autoBalance, bool friendlyFire, bool friendlyTags, bool friendlyFireWarning, bool showTracers, bool showTeamClays, bool allowAutoRange)
        {
            int value = 15887;

            if (autoBalance) value -= 4;
            if (friendlyFire) value -= 512;
            if (friendlyTags) value -= 1024;
            if (friendlyFireWarning) value -= 8;
            if (showTracers) value -= 1;
            if (showTeamClays) value += 32768;
            if (allowAutoRange) value += 65536;

            return value;
        }
        // Function: IsMapTeamBased, checks if the given game type is team-based and returns true or false.
        public static bool IsMapTeamBased(int gametype)
        {
            switch (gametype)
            {
                case 0: // DM
                    return false;
                case 1: // TDM
                    return true;
                case 3: //TKOTH
                    return true;
                case 4: // King of The Hill
                    return false;
                case 5: // Search and Destroy
                    return true;
                case 6: // Attack and Defend
                    return true;
                case 7: // Capture the Flag
                    return true;
                case 8: // Flagball
                    return true;
                default: // all others are assumed to be invalid game types...
                    return false;
            }
        }
    }
}
