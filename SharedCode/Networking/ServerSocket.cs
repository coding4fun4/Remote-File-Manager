using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SharedCode.Networking
{
    struct SClientConnection
    {
        public CClientSocket ClientSocket;

        public int NextBufferSize;
        public byte[] Buffer;
    }

    class CServerSocket
    {
        public delegate void delOnClientConnected(CClientSocket ClientSock);
        public event delOnClientConnected OnClientConnected;

        public delegate void delOnClientReceivedBuffer(CClientSocket ClientSock, byte[] buffer);
        public event delOnClientReceivedBuffer OnClientReceivedBuffer;

        public delegate void delOnClientDisconnect(CClientSocket ClientSock);
        public event delOnClientDisconnect OnClientDisconnect;

        private int Port { get; }

        private Socket ServerSock { get; set; }

        public CServerSocket(int port)
        {
            this.Port = port;
        }

        public void StartAcceptingConnections()
        {
            ServerSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSock.Bind((EndPoint)new IPEndPoint(IPAddress.Any, Port));
            ServerSock.Listen(100);

            ServerSock.BeginAccept(BeginAccept_Callback, null);
        }

        void BeginAccept_Callback(IAsyncResult iar)
        {
            try
            {
                Socket ClientSock = ServerSock.EndAccept(iar);

                CClientSocket ClientSocket = new CClientSocket(ClientSock);

                if (OnClientConnected != null)
                    OnClientConnected.Invoke(ClientSocket);

                //Accept the next connection
                ServerSock.BeginAccept(BeginAccept_Callback, null);

                //Start receiving data from new client
                SClientConnection ClientConnection = new SClientConnection
                {
                    ClientSocket = ClientSocket,
                    NextBufferSize = -1,
                    Buffer = new byte[4]
                };

                ClientSock.BeginReceive(ClientConnection.Buffer, 0, 4, SocketFlags.None, BeginReceive_Callback, ClientConnection);
            }
            catch { }
        }
        static public bool FinishReceiveData(ref int DataReceived, Socket ClientSock, IAsyncResult iar)
        {
            int Received = 0;

            try
            {
                Received = ClientSock.EndReceive(iar);
            }
            catch { }

            return ((DataReceived = Received) > 0);
        }

        void HandleReceivedFullBuffer(SClientConnection ClientConnection)
        {
            if(OnClientReceivedBuffer != null)
            {
                OnClientReceivedBuffer.Invoke(ClientConnection.ClientSocket, ClientConnection.Buffer);
            }
        }

        void HandleReceivedBuffer(int DataReceived, SClientConnection ClientConnection)
        {
            //We have received only a part of the buffer
            if (DataReceived != ClientConnection.Buffer.Length)
            {
                //Attempt to receive the full buffer
                ClientConnection.ClientSocket.GetSocket().BeginReceive(ClientConnection.Buffer, DataReceived, ClientConnection.Buffer.Length - DataReceived, SocketFlags.None, BeginReceive_Callback, ClientConnection);
            }
            else
            {
                //At this stage, we only know the buffer size
                if (ClientConnection.NextBufferSize == -1)
                {
                    //Receive the actual buffer
                    ClientConnection.NextBufferSize = BitConverter.ToInt32(ClientConnection.Buffer, 0);
                    ClientConnection.Buffer = new byte[ClientConnection.NextBufferSize];
                    ClientConnection.ClientSocket.GetSocket().BeginReceive(ClientConnection.Buffer, 0, ClientConnection.Buffer.Length, SocketFlags.None, BeginReceive_Callback, ClientConnection);
                }
                else
                {
                    HandleReceivedFullBuffer(ClientConnection);

                    //Receive the next buffer size
                    ClientConnection.NextBufferSize = -1;
                    ClientConnection.Buffer = new byte[4];
                    ClientConnection.ClientSocket.GetSocket().BeginReceive(ClientConnection.Buffer, 0, 4, SocketFlags.None, BeginReceive_Callback, ClientConnection);
                }
            }
        }

        void HandleDisconnect(CClientSocket ClientSocket)
        {
            if(OnClientDisconnect != null)
            {
                OnClientDisconnect.Invoke(ClientSocket);
            }
        }

        void BeginReceive_Callback(IAsyncResult iar)
        {
            try
            {
                SClientConnection ClientConnection = (SClientConnection)iar.AsyncState;

                int DataReceived = 0;

                if (FinishReceiveData(ref DataReceived, ClientConnection.ClientSocket.GetSocket(), iar))
                {
                    HandleReceivedBuffer(DataReceived, ClientConnection);
                }
                else HandleDisconnect(ClientConnection.ClientSocket);
            }
            catch { }
        }
    }
}
