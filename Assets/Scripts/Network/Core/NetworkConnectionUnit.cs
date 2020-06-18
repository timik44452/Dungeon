using System.Net.Sockets;

namespace Networking
{
    [System.Serializable]
    public class NetworkConnectionUnit
    {
        public readonly int Id;

        //TODO:Need realization
        public string Address => (connection != null) ? connection.ToString() : "None";
        public bool IsConnected => connection != null && connection.Connected;

        private readonly TcpClient connection;


        #region Constructors
        public NetworkConnectionUnit(bool IsAsyncMode, TcpClient connection)
        {
            this.connection = connection;
        }
        #endregion

        public NetworkStream GetStream()
        {
            return connection.GetStream();
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                connection.Close();
            }
        }

        #region Redefines
        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object obj)
        {
            NetworkConnectionUnit unit = obj as NetworkConnectionUnit;

            return unit != null && unit.Id == Id;
        }
        public override string ToString()
        {
            return $"Connection unit {Id} {Address}";
        }
        #endregion
    }
}
