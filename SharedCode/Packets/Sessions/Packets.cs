using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Packets.Sessions
{
    public enum ESessionPackets : byte
    {
        Introduction = 0x0,
        Packet = 0x1
    }

    [Serializable]
    public struct SSessionPacket
    {
        public int ControllerKey;
        public int ClientHandle;
        public int SessionId;
        public ESessionPackets Packet;
        public byte[] data;
    }
}
