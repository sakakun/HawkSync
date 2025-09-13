using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using System.Windows.Forms;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class consoleInstanceManager
    {
        public static consoleInstanceInterface Implementation { get; set; }

        public static void updateConsoleWindow(string AuthToken) => Implementation.updateConsoleWindow(AuthToken);

    }
}
