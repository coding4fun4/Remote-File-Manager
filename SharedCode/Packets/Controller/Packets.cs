using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Packets.Controller
{
    public enum EControllerPackets : byte
    {
        Introduction = 0x0,
        SyncData = 0x1,
        Session = 0x2
    }

    [Serializable]
    public struct SControllerIntroduction
    {
        public string Password;
    }

    [Serializable]
    public struct SControllerAnswer
    {
        public bool IsAuthorized;
        public int key;
    }

    [Serializable]
    public struct SClient
    {
        public int SocketHandle;
        public string IPAddress;
        public string UserName;
        public string OperatingSystem;
    }

    public enum EDataSyncType : byte
    {
        Set = 0x0,
        Remove = 0x1
    }

    public enum ESyncList : byte
    {
        Clients = 0x0
    }

    [Serializable]
    public struct SDataSync
    {
        public int index;
        public EDataSyncType type;
        public ESyncList list;
        public byte[] data;
    }
}
