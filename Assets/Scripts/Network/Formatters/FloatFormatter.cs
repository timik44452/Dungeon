using System;
using Networking.Exceptions;

namespace Networking.Formatters
{
    public class FloatFormatter : INetworkDataFormatter
    {
        public Type TargetType => typeof(float);

        public NetworkData GetData(int header, object value)
        {
            if (value.GetType() == TargetType)
            {
                return GetNetworkData(header, Hasher.GetHash(TargetType.Name), BitConverter.GetBytes((float)value));
            }
            else
            {
                throw new FormatException($"object has the wrong type");
            }
        }

        public object GetExampleValue()
        {
            return 0.7852F;
        }

        public object GetValue(NetworkData value)
        {
            return BitConverter.ToSingle(value.Data, 0);
        }

        private NetworkData GetNetworkData(int Header, int Type, byte[] buffer)
        {
            NetworkData result = new NetworkData(Header, Type, buffer.Length);
            buffer.CopyTo(result.Data, 0);
            return result;
        }
    }
}
