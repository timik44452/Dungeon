using System;
using UnityEngine;

namespace Networking
{
    public class Vector3Formatter : INetworkDataFormatter
    {
        public Type TargetType => typeof(Vector3);

        public NetworkData GetData(int header, object value)
        {
            if (value.GetType() == TargetType)
            {
                byte[] buffer = new byte[12];
                Vector3 vector = (Vector3)value;

                BitConverter.GetBytes(vector.x).CopyTo(buffer, 0);
                BitConverter.GetBytes(vector.y).CopyTo(buffer, 4);
                BitConverter.GetBytes(vector.z).CopyTo(buffer, 8);

                return GetNetworkData(header, Hasher.GetHash(TargetType.Name), buffer);
            }
            else
            {
                throw new FormatException($"object has the wrong type");
            }
        }

        public object GetValue(NetworkData value)
        {
            return new Vector3(
                BitConverter.ToSingle(value.Data, 0),
                BitConverter.ToSingle(value.Data, 4),
                BitConverter.ToSingle(value.Data, 8));
        }

        private NetworkData GetNetworkData(int Header, int Type, byte[] buffer)
        {
            NetworkData result = new NetworkData(Header, Type, buffer.Length);
            buffer.CopyTo(result.Data, 0);
            return result;
        }
    }
}