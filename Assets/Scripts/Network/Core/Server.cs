using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Networking;

public static class Server
{
    public static bool Runing { get; private set; }
    public static INetworkCommunicator Communicator { get; private set; }
    public static List<NetworkConnectionUnit> Connections { get; private set; } = new List<NetworkConnectionUnit>();

    private static event Action<NetworkConnectionUnit> onConnected;
    private static event Action<NetworkConnectionUnit> onDisconnected;

    private static TcpListener listener;
    private static CancellationTokenSource tokenSource;


    public static void Start(int port)
    {
        try
        {
            Communicator = new NetworkCommunicator();
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            StartOperation(1, CheckSend);
            StartOperation(1, CheckRecieve);
            StartOperation(600, CheckConnectionsState);

            RuntimeManager.RegisterApplicationClosingCallback(Stop);

            Runing = true;
        }
        catch
        {
            Stop();
        }
    }

    public static void Stop()
    {
        tokenSource?.Cancel();

        listener.Stop();

        Runing = false;
    }

    public static INetworkCommunicator CreateCommunicator(int port)
    {
        Start(port);

        return Communicator;
    }

    public static void RegisterDisconnectionCallback(Action<NetworkConnectionUnit> action)
    {
        onDisconnected += action;
    }

    public static void RegisterConnectionCallback(Action<NetworkConnectionUnit> action)
    {
        onConnected += action;
    }

    private static void OnDisconnected(NetworkConnectionUnit unit)
    {
        Connections.Remove(unit);
        onDisconnected?.Invoke(unit);
    }

    private static async void CheckConnectionsState()
    {
        var incomingConnection = await listener.AcceptTcpClientAsync();

        lock (Connections)
        {
            NetworkConnectionUnit connectionUnit = new NetworkConnectionUnit(false, incomingConnection);

            Connections.FindAll(x => !x.IsConnected).ForEach(OnDisconnected);
            Connections.Add(connectionUnit);

            onConnected?.Invoke(connectionUnit);
        }
    }

    private static void CheckSend()
    {
        NetworkData data = Communicator.GetSendingItemAndRemove();

        if (data == null)
        {
            return;
        }

        lock (Connections)
        {
            Protocol.Send(data, Connections.ToArray());
        }
    }

    private static void CheckRecieve()
    {
        lock (Connections)
        {
            foreach (NetworkConnectionUnit connection in Connections)
            {
                NetworkStream stream = connection.GetStream();

                if (!Runing | !stream.DataAvailable)
                {
                    continue;
                }

                NetworkData data = Protocol.Recieve(stream);

                Communicator.InvokeRecieve(data);

                Communicator.InvokeRecieve(data);
            }
        }
    }

    private static void StartOperation(int timeout, Action operation)
    {
        if (operation == null)
        {
            return;
        }

        if (tokenSource == null)
        {
            tokenSource = new CancellationTokenSource();
        }

        new Thread(
            () =>
            {
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    operation.Invoke();
                    Thread.Sleep(timeout);
                }
            }
            ).Start();
    }
}