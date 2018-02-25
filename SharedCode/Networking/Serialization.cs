using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedCode.Networking
{
    class CSerialization
    {
        static public T Deserialize<T>(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return ProtoBuf.Serializer.Deserialize<T>(ms);
            }
        }

        static public byte[] Serialize<T>(T buffer)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(ms, buffer);

                return ms.ToArray();
            }
        }
    }
}
