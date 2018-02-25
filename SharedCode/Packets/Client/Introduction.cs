using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace SharedCode.Packets.Client
{
    [ProtoContract]
    public struct SClientIntroduction
    {
        [ProtoMember(1)]
        public string UserName;
        [ProtoMember(2)]
        public string OperatingSystem;
    }
}
