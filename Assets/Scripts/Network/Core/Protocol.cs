using System;
using System.Net.Sockets;

namespace Networking
{
    public static class Protocol
    {
        public static void Send(NetworkData data, params NetworkConnectionUnit[] units)
        {
            byte[] buffer = new byte[12 + data.Data.Length];

            data.GetHeaderBytes().CopyTo(buffer, 0);
            data.Data.CopyTo(buffer, 12);

            foreach (NetworkConnectionUnit connectionUnit in units)
            {
                connectionUnit.GetStream().Write(buffer, 0, buffer.Length);
            }
        }

        public static NetworkData Recieve(NetworkStream stream)
        {
            int position = 0;
            byte[] header = new byte[12];

            // Wait header
            do
            {
                position += stream.Read(header, position, 12 - position);
            }
            while (position < 12);

            position = 0;
            NetworkData data = NetworkData.ContainerFromBytes(header);

            byte[] buffer = new byte[512];

            // Recieve data
            do
            {
                position += stream.Read(data.Data, position, Math.Min(data.Size - position, buffer.Length));
            }
            while (position < data.Size);

            return data;
        }
    }
}
