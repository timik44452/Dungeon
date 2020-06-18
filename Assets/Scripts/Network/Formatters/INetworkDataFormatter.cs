using System;


namespace Networking
{
    public interface INetworkDataFormatter
    {
        Type TargetType { get; }

        NetworkData GetData(int Header, object value);

        object GetValue(NetworkData value);
    }
}
