using System;

namespace Networking.Formatters
{
    public class Int32Formatter : INetworkDataFormatter
    {
        public Type TargetType => typeof(int);

        public NetworkData GetData(int header, object value)
        {
            if (value.GetType() == TargetType)
            {
                return GetNetworkData(header, Hasher.GetHash(TargetType.Name), BitConverter.GetBytes((int)value));
            }
            else
            {
                throw new FormatException($"object has the wrong type");
            }
        }

        public object GetValue(NetworkData value)
        {
            return BitConverter.ToInt32(value.Data, 0);
        }

        private NetworkData GetNetworkData(int Header, int Type, byte[] buffer)
        {
            NetworkData result = new NetworkData(Header, Type, buffer.Length);
            buffer.CopyTo(result.Data, 0);
            return result;
        }
    }
}
