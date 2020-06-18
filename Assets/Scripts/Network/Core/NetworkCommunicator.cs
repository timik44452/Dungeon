using System;
using System.Collections.Generic;

namespace Networking
{
    public class NetworkCommunicator : INetworkCommunicator
    {
        public int SendingCount => send_buffer.Count;
        public NetworkDataFactory DataFactory { get; } = new NetworkDataFactory();

        private const int SendingBufferLength = 16;

        private int broadcast_hash = Hasher.GetHash("@_BROADCAST");

        private Dictionary<int, List<Action<int, object>>> recievers = new Dictionary<int, List<Action<int, object>>>();
        private List<NetworkData> send_buffer = new List<NetworkData>();

        public void Send(int id, object data)
        {
            Send(id, data, null);
        }

        public void Send(int id, object data, NetworkConnectionUnit target)
        {
            lock (send_buffer)
            {
                if (send_buffer.Count < SendingBufferLength)
                {
                    NetworkData networkData = DataFactory.GetData(id, data);

                    send_buffer.RemoveAll(x => x.Header == networkData.Header);
                    send_buffer.Add(networkData);
                }
                else
                {
                    throw new Exception($"Sendig buffer overflow");
                }
            }
        }

        public NetworkData GetSendingItemAndRemove()
        {
            if (send_buffer.Count == 0)
            {
                return null;
            }

            lock (send_buffer)
            {
                NetworkData buffer = send_buffer[0];

                send_buffer.RemoveAt(0);

                return buffer;
            }
        }

        public void RegisterRecieveCallback<T>(int id, Action<T> action)
        {
            RegisterRecieveCallback(id, _object => action?.Invoke((T)_object));
        }

        public void RegisterRecieveCallback(int id, Action<object> action)
        {
            lock (recievers)
            {
                if (!recievers.ContainsKey(id))
                {
                    recievers.Add(id, new List<Action<int, object>>());
                }

                recievers[id].Add((_id, _value) => action?.Invoke(_value));
            }
        }

        public void RegisterRecieveCallback(int id, Action<int, object> action)
        {
            lock (recievers)
            {
                if (!recievers.ContainsKey(id))
                {
                    recievers.Add(id, new List<Action<int, object>>());
                }

                recievers[id].Add(action);
            }
        }

        public void InvokeRecieve(NetworkData data, bool useDispather = true)
        {
            lock (recievers)
            {
                if (useDispather)
                    UnityTimer.ThreadDispathcher.Invoke(data.Header, () => InvokeRecieveWithoutDispatcher(data));
                else
                    InvokeRecieveWithoutDispatcher(data);
            }
        }

        public void InvokeRecieveWithoutDispatcher(NetworkData data)
        {
            object _value = DataFactory.GetValue(data);

            List<Action> callbacks = new List<Action>();

            foreach (var reciever in recievers)
            {
                if (reciever.Key != broadcast_hash &&
                    reciever.Key != data.Header)
                {
                    continue;
                }

                reciever.Value.ForEach(callback => callbacks.Add(() => callback?.Invoke(data.Header, _value)));
            }

            foreach (var callback in callbacks)
            {
                callback.Invoke();
            }
        }
    }
}
