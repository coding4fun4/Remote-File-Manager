using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Controller_WinForms.Connection;
using SharedCode.Packets.Controller;
using SharedCode.Packets.Sessions;

namespace Controller_WinForms.Controller_Session
{
    interface IControllerSession
    {
        CConnection Connection { get; set; }

        int Id { get; set; }

        SClient Client { get; set; }

        void OnPacketReceived(ESessionPackets packet, byte[] arguments);

        void StartSession();
    }
}
