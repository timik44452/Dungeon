using Networking;
using System;

public static class ClientManager
{
    public static INetworkCommunicator Communicator
    {
        get
        {
            CreateClientIfNotCreated();

            return s_client.Communicator;
        }
    }

    public static string IP = "127.0.0.1";
    public static int Port = 25000;

    private static ILogger s_currentLogger = null;
    private static Client s_client = null;
    private static DateTime old_time;

    private static void CreateClientIfNotCreated()
    {
        if ((DateTime.Now - old_time).TotalSeconds < 5)
            return;

        old_time = DateTime.Now;


        if (s_currentLogger == null)
        {
            s_currentLogger = new UnityLogger();
        }

        if (s_client == null || s_client.IsConnected == false)
        {
            s_client = new Client(s_currentLogger);
            s_client.Connect(IP, Port);
        }
    }
}
