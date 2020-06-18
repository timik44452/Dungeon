namespace Networking
{
    public class NetworkData
    {
        public readonly int Header;
        public readonly int Type;
        public readonly int Size;
        
        public readonly byte[] Data;


        public NetworkData(int Header, int Type, int Size)
        {
            this.Header = Header;
            this.Size = Size;
            this.Type = Type;

            Data = new byte[Size];
        }

        public byte[] GetHeaderBytes()
        {
            byte[] result = new byte[12];

            System.BitConverter.GetBytes(Header).CopyTo(result, 0);
            System.BitConverter.GetBytes(Type).CopyTo(result, 4);
            System.BitConverter.GetBytes(Size).CopyTo(result, 8);

            return result;
        }

        public static NetworkData ContainerFromBytes(byte[] data)
        {
            if (data.Length != 12)
            {
                throw new System.Exception($"Data has incorrect size");
            }

            return new NetworkData(
                System.BitConverter.ToInt32(data, 0), 
                System.BitConverter.ToInt32(data, 4),
                System.BitConverter.ToInt32(data, 8));
        }
    }
}