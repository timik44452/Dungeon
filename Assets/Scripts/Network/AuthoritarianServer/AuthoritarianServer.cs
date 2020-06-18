using UnityEngine;

using Networking;

public class AuthoritarianServer : MonoBehaviour
{
    public int port;

    private void Awake()
    {
        Server.Start(port);

        int broadcast_hash = Hasher.GetHash("@_BROADCAST");

        Server.Communicator.RegisterRecieveCallback(broadcast_hash, Redirect);
    }

    private void Redirect(int id, object value)
    {
        Server.Communicator.Send(id, value);
    }
}
