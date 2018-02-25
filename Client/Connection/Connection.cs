using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedCode.Networking;
using SharedCode.Packets.Client;

namespace Client.Connection
{
    delegate void delOnPacketCallback(byte[] arguments);

    struct SPacketCallback
    {
        public EClientPackets packet;
        public delOnPacketCallback callback;
    }

    class CConnection
    {
        private CClientSocket ClientSocket { get; }

        private List<SPacketCallback> PacketCallbacks { get; }

        public CConnection(string host, int port)
        {
            ClientSocket = new CClientSocket(host, port);
            ClientSocket.OnClientConnected += ClientSocket_OnClientConnected;
            ClientSocket.OnClientDisconnected += ClientSocket_OnClientDisconnected;
            ClientSocket.OnClientReceivedData += ClientSocket_OnClientReceivedData;

            PacketCallbacks = new List<SPacketCallback>();
            InitPacketCallbacks();
        }

        void InitPacketCallbacks()
        {

        }

        private void ClientSocket_OnClientReceivedData(byte[] buffer)
        {
            byte packet = buffer[0];
            byte[] arguments = null;

            if(buffer.Length > 1)
            {
                arguments = new byte[buffer.Length - 1];
                Buffer.BlockCopy(buffer, 1, arguments, 0, arguments.Length);
            }


        }

        private void ClientSocket_OnClientDisconnected()
        {
           
        }

        private void ClientSocket_OnClientConnected()
        {
            //Tell the server that this is a client
            byte[] buffer = BitConverter.GetBytes((int)2);
            ClientSocket.SendBuffer(buffer);

            SClientIntroduction ClientIntroduction = new SClientIntroduction
            {
                UserName = Environment.UserName,
                OperatingSystem = new Microsoft.VisualBasic.Devices.ComputerInfo().OSFullName //sure there are better ways, but for the sake of open-source - i don't really care
            };

            //Send the introduction
            ClientSocket.SendPacket<SClientIntroduction>((byte)EClientPackets.Introduction, ClientIntroduction);
        }

        public void StartConnectingToServer()
        {
            ClientSocket.StartConnectingToServer();
        }
    }
}
