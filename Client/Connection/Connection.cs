using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedCode.Networking;
using SharedCode.Packets.Client;
using SharedCode.Packets.Sessions;

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
            PacketCallbacks.Add(new SPacketCallback() { packet = EClientPackets.Session, callback = OnSessionCallback });
        }

        void OnSessionCallback(byte[] arguments)
        {
            SSessionPacket SessionPacket = CSerialization.Deserialize<SSessionPacket>(arguments);

            if(SessionPacket.Packet == ESessionPackets.Introduction)
            {
                ClientSocket.SendPacket<SSessionPacket>((byte)EClientPackets.Session, SessionPacket);
            }
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

            foreach(SPacketCallback callback in PacketCallbacks)
            {
                if(callback.packet == (EClientPackets)packet)
                {
                    callback.callback(arguments);
                    break;
                }
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
