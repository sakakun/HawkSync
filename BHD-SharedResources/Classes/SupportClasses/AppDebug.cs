using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.SupportClasses
{
    public static class AppDebug
    {

        public static void Log(string classString, string debugMessage)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine($"[DEBUG][{classString}] {DateTime.Now}: {debugMessage}");
            }
        }
    }
}
