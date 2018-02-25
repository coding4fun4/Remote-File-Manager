using System;
using System.Collections.Generic;
using System.Text;

using ProtoBuf;

namespace SharedCode.Packets.Controller
{
    enum EControllerPackets : byte
    {
        Introduction = 0x0,
        SyncData = 0x1,
        Session = 0x2
    }

    [ProtoContract]
    public struct SControllerIntroduction
    {
        [ProtoMember(1)]
        public string Password;
    }

    [ProtoContract]
    public struct SControllerAnswer
    {
        [ProtoMember(1)]
        public bool IsAuthorized;
        [ProtoMember(2)]
        public string key;
    }

    [ProtoContract]
    public struct SClient
    {
        [ProtoMember(1)]
        public int SocketHandle;
        [ProtoMember(2)]
        public string IPAddress;
        [ProtoMember(3)]
        public string UserName;
        [ProtoMember(4)]
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

    [ProtoContract]
    public struct SDataSync
    {
        [ProtoMember(1)]
        public int index;
        [ProtoMember(2)]
        public EDataSyncType type;
        [ProtoMember(3)]
        public ESyncList list;
        [ProtoMember(4)]
        public byte[] data;
    }
}
