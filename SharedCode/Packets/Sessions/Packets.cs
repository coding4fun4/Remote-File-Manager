using System;
using System.Collections.Generic;
using System.Text;

using ProtoBuf;

namespace SharedCode.Packets.Sessions
{
    public enum ESessionPackets : byte
    {
        Introduction = 0x0,
        SyncData = 0x1
    }

    [ProtoContract]
    public struct SSessionPacket
    {
        [ProtoMember(1)]
        public int ControllerKey;

        [ProtoMember(2)]
        public int SessionId;

        [ProtoMember(3)]
        public ESessionPackets Packet;

        [ProtoMember(4)]
        public byte[] data;
    }
}
