using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SharedCode.Networking
{
    struct SReceivedData
    {
        public int NextBufferSize;
        public byte[] Buffer;
    }

    class CClientSocket
    {
        public delegate void delOnClientConnected();
        public event delOnClientConnected OnClientConnected;

        public delegate void delOnClientDisconnected();
        public event delOnClientDisconnected OnClientDisconnected;

        public delegate void delOnClientReceivedData(byte[] buffer);
        public event delOnClientReceivedData OnClientReceivedData;

        private Socket ClientSock { get; set; }

        private IPEndPoint Destination { get; }

        public CClientSocket(Socket ClientSock)
        {
            this.ClientSock = ClientSock;
        }

        public CClientSocket(string host, int port)
        {
            Destination = new IPEndPoint(IPAddress.Parse(host), port);
        }

        public void StartConnectingToServer()
        {
            ClientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSock.BeginConnect((EndPoint)Destination, BeginConnect_Callback, null);
        }

        public void Disconnect()
        {
            ClientSock.Close();
        }

        string[] GetIPv4Data()
        {
            IPEndPoint localEndPoint = (IPEndPoint)ClientSock.LocalEndPoint;

            string buffer = localEndPoint.ToString();

            return buffer.Split(':');
        }

        public string GetIPv4()
        {
            return GetIPv4Data()[0];
        }

        public int GetHandle()
        {
            string handle = GetIPv4Data()[1];

            return int.Parse(handle);
        }

        void Reconnect()
        {
            Thread.Sleep(1000);
            StartConnectingToServer();
        }

        void BeginConnect_Callback(IAsyncResult iar)
        {
            try
            {
                ClientSock.EndConnect(iar);

                //At this point we are connected to the destination

                if (OnClientConnected != null)
                    OnClientConnected.Invoke();

                SReceivedData ReceivedData = new SReceivedData
                {
                    NextBufferSize = -1,
                    Buffer = new byte[4]
                };

                ClientSock.BeginReceive(ReceivedData.Buffer, 0, 4, SocketFlags.None, BeginReceive_Callback, ReceivedData);
            }
            catch
            {
                Reconnect();
            }
        }

        void HandleDisconnect()
        {
            if(OnClientDisconnected != null)
            {
                OnClientDisconnected.Invoke();
            }

            Reconnect();
        }

        void HandleReceivedBuffer(int ReceivedDataSize, SReceivedData ReceivedData)
        {
            if(ReceivedDataSize != ReceivedData.Buffer.Length)
            {
                ClientSock.BeginReceive(ReceivedData.Buffer, ReceivedDataSize, ReceivedData.Buffer.Length - ReceivedDataSize, SocketFlags.None, BeginReceive_Callback, ReceivedData);
            }
            else
            {
                if(ReceivedData.NextBufferSize == -1)
                {
                    ReceivedData.NextBufferSize = BitConverter.ToInt32(ReceivedData.Buffer, 0);
                    ReceivedData.Buffer = new byte[ReceivedData.NextBufferSize];
                    ClientSock.BeginReceive(ReceivedData.Buffer, 0, ReceivedData.NextBufferSize, SocketFlags.None, BeginReceive_Callback, ReceivedData);
                }
                else
                {
                    if(OnClientReceivedData != null)
                        OnClientReceivedData.Invoke(ReceivedData.Buffer);

                    ReceivedData.NextBufferSize = -1;
                    ReceivedData.Buffer = new byte[4];
                    ClientSock.BeginReceive(ReceivedData.Buffer, 0, ReceivedData.Buffer.Length, SocketFlags.None, BeginReceive_Callback, ReceivedData);
                }
            }
        }

        void BeginReceive_Callback(IAsyncResult iar)
        {
            try
            {
                SReceivedData ReceivedData = (SReceivedData)iar.AsyncState;

                int DataReceived = 0;

                if (CServerSocket.FinishReceiveData(ref DataReceived, ClientSock, iar))
                {
                    HandleReceivedBuffer(DataReceived, ReceivedData);   
                }
                else HandleDisconnect();
            }
            catch
            {
                HandleDisconnect();
            }
        }

        public Socket GetSocket()
        {
            return ClientSock;
        }

        public void SendBuffer(byte[] buffer)
        {
            ClientSock.Send(BitConverter.GetBytes(buffer.Length));
            ClientSock.Send(buffer);
        }

        public void SendPacket(byte packet, byte[] arguments)
        {
            byte[] buffer = new byte[arguments == null ? 1 : arguments.Length + 1];
            buffer[0] = packet;

            if(arguments != null)
            {
                Buffer.BlockCopy(arguments, 0, buffer, 1, arguments.Length);
            }

            SendBuffer(buffer);
        }

        public void SendPacket<T>(byte packet, T arguments)
        {
            byte[] buffer = CSerialization.Serialize<T>(arguments);

            SendPacket(packet, buffer);
        }
    }
}
