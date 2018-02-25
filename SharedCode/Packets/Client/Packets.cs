using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Packets.Client
{
    enum EClientPackets : byte
    {
        Introduction = 0x0,
        Session = 0x1
    }
}
