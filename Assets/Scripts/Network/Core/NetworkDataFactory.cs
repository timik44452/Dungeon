using Networking.Formatters;
using Networking.Exceptions;
using System.Collections.Generic;

namespace Networking
{
    public class NetworkDataFactory
    {
        private List<INetworkDataFormatter> formatters = new List<INetworkDataFormatter>(new INetworkDataFormatter[]
        {
            new Vector3Formatter(),
            new StringFormatter(),
            new Int32Formatter(),
            new FloatFormatter()
        });

        public INetworkDataFormatter[] GetFormatters()
        {
            return formatters.ToArray();
        }

        public NetworkData GetData(int Header, object data)
        {
            if (data == null)
            {
                return null;
            }

            INetworkDataFormatter formatter = formatters.Find(x => data.GetType() == x.TargetType);

            if (formatter != null)
            {
                return formatter.GetData(Header, data);
            }
            else
            {
                throw new FormatterException($"Type {data.GetType()} hasn't formatter");
            }
        }

        public object GetValue(NetworkData networkData)
        {
            if (networkData == null)
            {
                return null;
            }

            INetworkDataFormatter formatter = formatters.Find(x => Hasher.GetHash(x.TargetType.Name) == networkData.Type);

            if (formatter != null)
            {
                return formatter.GetValue(networkData);
            }
            else
            {
                throw new FormatterException($"Incoming type hasn't formatter");
            }
        }

        public void RegisterFormatter(INetworkDataFormatter formatter)
        {
            if (formatters == null)
            {
                formatters = new List<INetworkDataFormatter>();
            }

            if (!formatters.Contains(formatter))
                formatters.Add(formatter);
        }
    }
}