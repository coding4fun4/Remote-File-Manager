using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NetSerializer;

namespace SharedCode.Networking
{
    class CSerialization
    {
        static public T Deserialize<T>(byte[] buffer)
        {
            Serializer s = new Serializer(new List<Type>() { typeof(T) });

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                s.DeserializeDirect<T>(ms, out T result);

                return result;
            }
        }

        static public byte[] Serialize<T>(T buffer)
        {
            Serializer s = new Serializer(new List<Type>() { typeof(T) });

            using (MemoryStream ms = new MemoryStream())
            {
                s.SerializeDirect<T>(ms, buffer);

                return ms.ToArray();
            }
        }
    }
}
