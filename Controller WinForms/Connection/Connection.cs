using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using SharedCode.Networking;
using SharedCode.Packets.Controller;
using SharedCode.Packets.Sessions;

namespace Controller_WinForms.Connection
{
    delegate void delOnPacketCallback(byte[] arguments);

    struct SPacketCallback
    {
        public EControllerPackets packet;
        public delOnPacketCallback callback;
    }

    public delegate void delOnSyncCallback(EDataSyncType type, int index, object data);

    public struct SSyncCallback
    {
        public ESyncList list;
        public delOnSyncCallback callback;
    }

    public class CConnection
    {
        public delegate void delOnConnectionAuthorized();
        public event delOnConnectionAuthorized OnConnectionAuthorized;

        public delegate void delOnConnectionAuthorizationFailed();
        public event delOnConnectionAuthorizationFailed OnConnectionAuthorizationFailed;

        public delegate void delOnUpdateKey(string key);
        public event delOnUpdateKey OnUpdateKey;

        public delegate void delOnRemoveClient(int index);
        public event delOnRemoveClient OnRemoveClient;

        public delegate void delOnUpdateClient(int index, SClient Client);
        public event delOnUpdateClient OnUpdateClient;

        public delegate void delOnSessionPacketReceived(SSessionPacket SessionPacket);
        public event delOnSessionPacketReceived OnSessionPacketReceived;

        private CClientSocket ClientSocket { get; }

        private List<SPacketCallback> PacketCallbacks { get; }

        public List<SSyncCallback> SyncCallbacks { get; }

        public CConnection(string host, int port)
        {
            ClientSocket = new CClientSocket(host, port);
            ClientSocket.OnClientConnected += ClientSocket_OnClientConnected;
            ClientSocket.OnClientDisconnected += ClientSocket_OnClientDisconnected;
            ClientSocket.OnClientReceivedData += ClientSocket_OnClientReceivedData;

            PacketCallbacks = new List<SPacketCallback>();
            InitPacketCallbacks();

            SyncCallbacks = new List<SSyncCallback>();
            InitSyncCallbacks();
        }

        void InitPacketCallbacks()
        {
            PacketCallbacks.Add(new SPacketCallback() { packet = EControllerPackets.Introduction, callback = PacketIntroductionResponse });
            PacketCallbacks.Add(new SPacketCallback() { packet = EControllerPackets.SyncData, callback = PacketDataSyncResponse });
        }

        void InitSyncCallbacks()
        {
            SyncCallbacks.Add(new SSyncCallback() { list = ESyncList.Clients, callback = OnClientsSync });
        }

        void OnClientsSync(EDataSyncType type, int index, object data)
        {
            if (data.GetType() != typeof(SClient))
                return;

            if(type == EDataSyncType.Remove)
            {
                if(OnRemoveClient != null)
                {
                    OnRemoveClient.Invoke(index);
                }
            }
            else
            {
                if(OnUpdateClient != null)
                {
                    OnUpdateClient.Invoke(index, (SClient)data);
                }
            }
        }

        void PacketDataSyncResponse(byte[] arguments)
        {
            SDataSync DataSync = CSerialization.Deserialize<SDataSync>(arguments);

            foreach (SSyncCallback SyncCallback in SyncCallbacks)
            {
                if (SyncCallback.list == DataSync.list)
                {
                    SyncCallback.callback(DataSync.type, DataSync.index, DataSync.data);
                    break;
                }
            }
        }

        void PacketIntroductionResponse(byte[] arguments)
        {
            SControllerAnswer ControllerAnswer = CSerialization.Deserialize<SControllerAnswer>(arguments);

            if(ControllerAnswer.IsAuthorized)
            {
                if(OnConnectionAuthorized != null)
                {
                    OnConnectionAuthorized.Invoke();
                }
            }
            else
            {
                if(OnConnectionAuthorizationFailed != null)
                {
                    OnConnectionAuthorizationFailed.Invoke();
                }
            }

            if(OnUpdateKey != null)
            {
                OnUpdateKey.Invoke(ControllerAnswer.key);
            }
        }

        void PacketSessionResponse(byte[] arguments)
        {
            SSessionPacket SessionPacket = CSerialization.Deserialize<SSessionPacket>(arguments);

            if(OnSessionPacketReceived != null)
            {
                OnSessionPacketReceived.Invoke(SessionPacket);
            }
        }

        private void ClientSocket_OnClientReceivedData(byte[] buffer)
        {
            EControllerPackets packet = (EControllerPackets)buffer[0];
            byte[] arguments = null;

            if(buffer.Length > 1)
            {
                arguments = new byte[buffer.Length - 1];
                Buffer.BlockCopy(buffer, 1, arguments, 0, arguments.Length);
            }

            foreach(SPacketCallback callback in PacketCallbacks)
            {
                if(callback.packet == packet)
                {
                    callback.callback(arguments);
                    break;
                }
            }
        }

        private void ClientSocket_OnClientDisconnected()
        {
            throw new NotImplementedException();
        }

        private void ClientSocket_OnClientConnected()
        {
            //We must announce the server-side that this is a Controller connection
            int controller = 1;

            byte[] buffer = BitConverter.GetBytes(controller);
            ClientSocket.SendBuffer(buffer);

            //We need to authorize this connection as a controller.
            SControllerIntroduction ControllerIntroduction = new SControllerIntroduction
            {
                Password = Program.Password
            };

            ClientSocket.SendPacket<SControllerIntroduction>((byte)EControllerPackets.Introduction, ControllerIntroduction);
        }

        public void StartConnectingToServer()
        {
            ClientSocket.StartConnectingToServer();
        }
    }
}
