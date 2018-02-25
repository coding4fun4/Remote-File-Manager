using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Server;

using SharedCode;
using SharedCode.Networking;
using SharedCode.Packets.Client;
using SharedCode.Packets.Controller;

namespace Server.Connections
{
    enum EConnectionType : int
    {
        Unknown = 0,
        Controller = 1,
        Client = 2
    }

    struct SControllerInformation
    {
        public bool IsAuthorized;
        public List<SClient> Clients;
        public int Key;
    }

    struct SConnection
    {
        public EConnectionType ConnectionType;
        public CClientSocket ClientSocket;
        public object Information;
    }

    delegate void delOnPacketCallback(CClientSocket ClientSocket, int ConnectionIndex, byte[] arguments);

    struct SPacketCallback
    {
        public EConnectionType ConnectionType;
        public byte Packet;
        public delOnPacketCallback OnPacketCallback;
    }

    class CConnectionManager
    {
        private CServerSocket ServerSocket { get; }

        private List<SConnection> Connections { get; }

        private List<SPacketCallback> PacketCallbacks { get; }

        private Thread SynchronizationThread { get; }

        private Random RandomInstance { get; }

        public CConnectionManager(int port)
        {
            ServerSocket = new CServerSocket(port);
            ServerSocket.OnClientConnected += ServerSocket_OnClientConnected;
            ServerSocket.OnClientDisconnect += ServerSocket_OnClientDisconnect;
            ServerSocket.OnClientReceivedBuffer += ServerSocket_OnClientReceivedBuffer;

            Connections = new List<SConnection>();

            PacketCallbacks = new List<SPacketCallback>();
            InitPacketCallbacks();

            SynchronizationThread = new Thread(new ThreadStart(Synchronization_Thread));

            RandomInstance = new Random();
        }

        void Synchronization_Thread()
        {
            while(true)
            {
                lock(Connections)
                {
                    CSynchronization.Execute(Connections);
                }

                Thread.Sleep(1000);
            }
        }

        void InitPacketCallbacks()
        {
            PacketCallbacks.Add(new SPacketCallback() { ConnectionType = EConnectionType.Client, Packet = (byte)EClientPackets.Introduction, OnPacketCallback = OnClientIntroductionCallback });
            PacketCallbacks.Add(new SPacketCallback() { ConnectionType = EConnectionType.Controller, Packet = (byte)EControllerPackets.Introduction, OnPacketCallback = OnControllerIntroductionCallback });
        }

        void RemoveConnection(CClientSocket ClientSocket)
        {
            lock(Connections)
            {
                for(int i = 0; i < Connections.Count; i++)
                {
                    if(Connections[i].ClientSocket == ClientSocket)
                    {
                        Connections.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        void AddConnection(CClientSocket ClientSocket)
        {
            SConnection connection = new SConnection
            {
                ClientSocket = ClientSocket,
                ConnectionType = EConnectionType.Unknown
            };

            lock (Connections)
            {
                Connections.Add(connection);
            }
        }

        int GenerateKeyForController()
        {
            bool generated = false;

            int key = -1;

            while(!generated)
            {
                key = RandomInstance.Next();

                bool exists = false;

                foreach(SConnection c in Connections)
                {
                    if (c.ConnectionType != EConnectionType.Controller)
                        continue;

                    SControllerInformation information = (SControllerInformation)c.Information;

                    if(information.Key == key)
                    {
                        exists = true;
                        break;
                    }
                }

                if(!exists)
                {
                    generated = true;
                }
            }

            return key;
        }

        void OnControllerIntroductionCallback(CClientSocket ClientSocket, int ConnectionIndex, byte[] arguments)
        {
            SControllerIntroduction ControllerIntroduction = (SControllerIntroduction)CSerialization.Deserialize<SControllerIntroduction>(arguments);

            SControllerAnswer ControllerAnswer = new SControllerAnswer
            {
                IsAuthorized = false,
                key = -1
            };

            SConnection Connection = Connections[ConnectionIndex];

            if (ControllerIntroduction.Password != Program.ControllerPassword)
            {
                //Tell the controller that password was not accepted
                Connection.ClientSocket.SendPacket<SControllerAnswer>((byte)EControllerPackets.Introduction, ControllerAnswer);

                //At this stage the password is not valid, so we disconnect the invalid controller
                ClientSocket.Disconnect();
                return;
            }

            int key = GenerateKeyForController();
            
            //Authorize connection
            SControllerInformation ControllerInformation = (SControllerInformation)Connection.Information;
            ControllerInformation.IsAuthorized = true;
            ControllerInformation.Key = key;
            Connection.Information = (object)ControllerInformation;
            Connections[ConnectionIndex] = Connection;

            //Key for controlling clients
            ControllerAnswer.key = key;
            ControllerAnswer.IsAuthorized = true;

            //Tell the controller that he is authorized
            Connection.ClientSocket.SendPacket<SControllerAnswer>((byte)EControllerPackets.Introduction, ControllerAnswer);
        }

        void OnClientIntroductionCallback(CClientSocket ClientSocket, int ConnectionIndex, byte[] arguments)
        {
            SClientIntroduction ClientIntroduction = (SClientIntroduction)CSerialization.Deserialize<SClientIntroduction>(arguments);

            SConnection Connection = Connections[ConnectionIndex];
            Connection.Information = ClientIntroduction;
            Connections[ConnectionIndex] = Connection;
        }

        void HandleReceivedBuffer(CClientSocket ClientSocket, int ConnectionIndex, EConnectionType ConnectionType, byte[] buffer)
        {
            byte packet = buffer[0];

            byte[] arguments = null;

            if(buffer.Length > 1)
            {
                arguments = new byte[buffer.Length - 1];
                Buffer.BlockCopy(buffer, 1, arguments, 0, arguments.Length);
            }

            foreach(SPacketCallback PacketCallback in PacketCallbacks)
            {
                if (PacketCallback.ConnectionType != ConnectionType)
                    continue;

                if (PacketCallback.Packet != packet)
                    continue;

                PacketCallback.OnPacketCallback(ClientSocket, ConnectionIndex, arguments);
                break;
            }
        }

        private void ServerSocket_OnClientReceivedBuffer(CClientSocket ClientSocket, byte[] buffer)
        {
            lock(Connections)
            {
                for(int i = 0; i < Connections.Count; i++)
                {
                    if(Connections[i].ClientSocket == ClientSocket)
                    {
                        if (Connections[i].ConnectionType == EConnectionType.Unknown)
                        {
                            bool IsConnectionTypeParsed = false;

                            if (buffer.Length >= 4)
                            {
                                int ConnectionType = BitConverter.ToInt32(buffer, 0);

                                if (ConnectionType == (int)EConnectionType.Client || ConnectionType == (int)EConnectionType.Controller)
                                {
                                    SConnection Connection = Connections[i];
                                    Connection.ConnectionType = (EConnectionType)ConnectionType;

                                    if(Connection.ConnectionType == EConnectionType.Controller)
                                    {
                                        SControllerInformation ControllerInformation = new SControllerInformation
                                        {
                                            Clients = new List<SClient>()
                                        };

                                        Connection.Information = ControllerInformation;
                                    }

                                    Connections[i] = Connection;
                                    IsConnectionTypeParsed = true;
                                }
                            }

                            if (!IsConnectionTypeParsed)
                            {
                                //At this point we have received an invalid buffer
                                ClientSocket.Disconnect();
                            }
                            else if(IsConnectionTypeParsed && buffer.Length > 4)
                            {
                                //Initial parsing for understanding what kind of connection it is is complete but there is more data available
                                byte[] NextBuffer = new byte[buffer.Length - 4];
                                Buffer.BlockCopy(buffer, 4, NextBuffer, 0, NextBuffer.Length);
                                HandleReceivedBuffer(ClientSocket, i, Connections[i].ConnectionType, NextBuffer);
                            }
                        }
                        else HandleReceivedBuffer(ClientSocket, i, Connections[i].ConnectionType, buffer);

                        break;
                    }
                }
            }
        }

        private void ServerSocket_OnClientDisconnect(CClientSocket ClientSocket)
        {
            RemoveConnection(ClientSocket);
        }

        private void ServerSocket_OnClientConnected(CClientSocket ClientSocket)
        {
            AddConnection(ClientSocket);
        }

        public void StartAcceptingConnections()
        {
            //Start synchronization thread so Controller knows the clients.
            SynchronizationThread.Start();

            //Start accepting connections from both controller(s) and client(s).
            ServerSocket.StartAcceptingConnections();
        }
    }
}
