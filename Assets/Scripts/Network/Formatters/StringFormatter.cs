using System;

namespace Networking.Formatters
{
    public class StringFormatter : INetworkDataFormatter
    {
        public Type TargetType => typeof(string);

        public NetworkData GetData(int header, object value)
        {
            if (value.GetType() == TargetType)
            {
                return GetNetworkData(header, Hasher.GetHash(TargetType.Name), System.Text.Encoding.Default.GetBytes(value.ToString()));
            }
            else
            {
                throw new FormatException($"object has the wrong type");
            }
        }

        public object GetExampleValue()
        {
            return "Hello world";
        }

        public object GetValue(NetworkData value)
        {
            return System.Text.Encoding.Default.GetString(value.Data);
        }

        private NetworkData GetNetworkData(int Header, int Type, byte[] buffer)
        {
            NetworkData result = new NetworkData(Header, Type, buffer.Length);
            buffer.CopyTo(result.Data, 0);
            return result;
        }
    }
}
