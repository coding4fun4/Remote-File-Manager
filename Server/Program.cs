using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Connections;
using SharedCode.Packets.Controller;

namespace Server
{
    class Program
    {
        public static string ControllerPassword { get; } = "abcd";

        static CConnectionManager ConnectionManager { get; set; }

        static void Main(string[] args)
        {
            ConnectionManager = new CConnectionManager(44444);
            ConnectionManager.StartAcceptingConnections();

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
