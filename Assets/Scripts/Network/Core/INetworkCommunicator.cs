using System;

namespace Networking
{
    public interface INetworkCommunicator
    {
        NetworkDataFactory DataFactory { get; }

        void Send(int id, object data);
        void Send(int id, object data, NetworkConnectionUnit target);

        void RegisterRecieveCallback<T>(int id, Action<T> action);
        void RegisterRecieveCallback(int id, Action<object> action);
        void RegisterRecieveCallback(int id, Action<int, object> action);

        NetworkData GetSendingItemAndRemove();
        void InvokeRecieve(NetworkData data, bool usDispatcher = true);
    }
}
