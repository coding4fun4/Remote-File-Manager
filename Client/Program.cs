using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Client.Connection;

namespace Client
{
    class Program
    {
        static CConnection Connection { get; set; }

        static void Main(string[] args)
        {
            Connection = new CConnection("127.0.0.1", 44444);
            Connection.StartConnectingToServer();

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
