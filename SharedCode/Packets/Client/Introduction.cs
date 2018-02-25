using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Packets.Client
{
    [Serializable]
    public struct SClientIntroduction
    {
        public string UserName;
        public string OperatingSystem;
    }
}
