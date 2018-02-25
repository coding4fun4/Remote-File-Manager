using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedCode.Networking;
using SharedCode.Packets.Client;
using SharedCode.Packets.Controller;

namespace Server.Connections
{
    class CSynchronization
    {
        static SClient GetClientStructFromConnection(SConnection Connection)
        {
            SClientIntroduction ClientIntroduction = (SClientIntroduction)Connection.Information;

            SClient ClientInfo = new SClient
            {
                IPAddress = Connection.ClientSocket.GetIPv4(),
                SocketHandle = Connection.ClientSocket.GetHandle(),
                UserName = ClientIntroduction.UserName,
                OperatingSystem = ClientIntroduction.OperatingSystem
            };

            return ClientInfo;
        }

        static bool CheckSerialization<T>(T one, T two, out byte[] source)
        {
            source = CSerialization.Serialize<T>(one);
            byte[] destination = CSerialization.Serialize<T>(two);

            if (source.Length != destination.Length)
                return false;

            bool result = true;

            for(int i = 0; i < source.Length; i++)
            {
                if (source[i] != destination[i])
                {
                    result = false;
                    return false;
                }
            }

            return result;
        }

        static void SynchronizeStructArray<T>(ESyncList list, List<T> one, ref List<T> result, CClientSocket ClientSocket)
        {
            for(int i = 0; i < one.Count; i++)
            {
                bool check = true;

                if(i > (result.Count - 1))
                {
                    result.Add(one[i]);
                    check = false;
                }

                bool sync = true;
                byte[] source = null;

                if (check)
                {
                    if (CheckSerialization<T>(one[i], result[i], out source))
                        sync = false;
                }
                else
                {
                    source = CSerialization.Serialize<T>(one[i]);
                }

                if (sync)
                {
                    SDataSync DataSync = new SDataSync
                    {
                        index = i,
                        type = EDataSyncType.Set,
                        list = list,
                        data = source
                    };

                    ClientSocket.SendPacket<SDataSync>((byte)EControllerPackets.SyncData, DataSync);
                }
            }

            if(one.Count < result.Count)
            {
                int removed = 0;

                for(int i = one.Count; i < result.Count; i++)
                {
                    int index_correction = i - removed;
                    result.RemoveAt(index_correction);

                    SDataSync DataSync = new SDataSync
                    {
                        index = index_correction,
                        type = EDataSyncType.Remove,
                        list = list,
                        data = null
                    };

                    removed++;

                    ClientSocket.SendPacket<SDataSync>((byte)EControllerPackets.SyncData, DataSync);
                }
            }
        }

        static public void Execute(List<SConnection> Connections)
        {
            List<SClient> clients = new List<SClient>();

            for (int ClientIndex = 0; ClientIndex < Connections.Count; ClientIndex++)
            {
                if (Connections[ClientIndex].ConnectionType != EConnectionType.Client)
                    continue;

                if (Connections[ClientIndex].Information == null)
                    continue;

                SClient ClientInfo = GetClientStructFromConnection(Connections[ClientIndex]);

                clients.Add(ClientInfo);
            }

            for (int ConnectionIndex = 0; ConnectionIndex < Connections.Count; ConnectionIndex++)
            {
                if (Connections[ConnectionIndex].ConnectionType != EConnectionType.Controller)
                    continue;

                SConnection ControllerConnection = Connections[ConnectionIndex];
                SControllerInformation ControllerInformation = (SControllerInformation)ControllerConnection.Information;

                if (!ControllerInformation.IsAuthorized)
                    continue;

                SynchronizeStructArray<SClient>(ESyncList.Clients, clients, ref ControllerInformation.Clients, ControllerConnection.ClientSocket);

                ControllerConnection.Information = ControllerInformation;
                Connections[ConnectionIndex] = ControllerConnection;
            }
        }
    }
}
