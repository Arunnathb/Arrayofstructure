using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace newRTECreation
{
    class OSinterface
    {
        //detarmine if it is linux platform
        public static bool IsLinux()
        {
            var p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }
        //determine if it is windows platform.
        public static bool IsWindows()
        {
            var p = (int)Environment.OSVersion.Platform;
            return (p != 4) && (p != 6) && (p != 128);
        }
    }
}
