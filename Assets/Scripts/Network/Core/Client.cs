using System;
using System.Net.Sockets;
using System.Threading;

using Networking;

public class Client
{
    public bool IsConnected => connection != null && connection.IsConnected;

    public INetworkCommunicator Communicator { get; private set; }

    private ILogger logger;
    private NetworkConnectionUnit connection;
    private CancellationTokenSource tokenSource;

    public Client(ILogger logger)
    {
        this.logger = logger;
    }

    public void Connect(string ip, int port)
    {
        Communicator = new NetworkCommunicator();

        try
        {
            var client = new TcpClient();
            client.Connect(ip, port);
            //var async_operation = client.BeginConnect(ip, port, null, null);
            //bool beginConnectionResultComplete = async_operation.AsyncWaitHandle.WaitOne(300, true);

            //if (beginConnectionResultComplete)
            //{
            //    client.EndConnect(async_operation);

            //    connection = new NetworkConnectionUnit(true, client);

            //    if (IsConnected)
            //    {
            //        StartOperation(1, CheckSend);
            //        StartOperation(1, CheckRecieve);

            //        RuntimeManager.RegisterApplicationClosingCallback((s, e) => Disconnect());
            //    }
            //}
            //else
            //{
            //    client.Close();
            //}

            connection = new NetworkConnectionUnit(true, client);

            if (IsConnected)
            {
                StartOperation(1, CheckSend);
                StartOperation(1, CheckRecieve);

                RuntimeManager.RegisterApplicationClosingCallback(Disconnect);
            }
        }
        catch (Exception e)
        {
            logger?.Log(e.Message);
            Disconnect();
        }
    }

    public void Disconnect()
    {
        tokenSource?.Cancel();
        connection?.Disconnect();
    }

    private void CheckSend()
    {
        NetworkData data = Communicator.GetSendingItemAndRemove();

        if (data == null)
        {
            return;
        }

        Protocol.Send(data, connection);
    }

    private void CheckRecieve()
    {
        NetworkStream stream = connection.GetStream();

        if (!IsConnected | !stream.DataAvailable)
        {
            return;
        }

        NetworkData data = Protocol.Recieve(stream);

        Communicator.InvokeRecieve(data);
    }

    private void StartOperation(int timeout, Action operation)
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
