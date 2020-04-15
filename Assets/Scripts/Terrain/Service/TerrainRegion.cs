using System;
using System.IO;
using UnityEngine;

namespace Game.Terrain.Service
{
    public class TerrainRegion
    {
        public int X { get; }
        public int Z { get; }
        public int Width { get; }
        public int Length { get; }

        private byte[] heights;
        private byte[] types;

        public TerrainRegion(int X, int Z, int Width, int Length)
        {
            this.X = X;
            this.Z = Z;
            this.Width = Width;
            this.Length = Length;

            heights = new byte[Width * Length];
            types = new byte[Width * Length];
        }

        public TerrainRegion(string path)
        {
            string region_name = Path.GetFileNameWithoutExtension(path);
            string region_size = region_name.Replace("region", string.Empty);

            int.TryParse(region_size.Split('_')[0], out int x);
            int.TryParse(region_size.Split('_')[1], out int z);

            using (Stream reader = new StreamReader(path).BaseStream)
            {
                int size = (int)System.Math.Sqrt(reader.Length * 0.5F);

                Width = size;
                Length = size;

                types = new byte[size * size];
                heights = new byte[size * size];

                reader.Read(heights, 0, heights.Length);
                reader.Read(types, 0, types.Length);
            }

            X = x;
            Z = z;
        }

        public float GetHeight(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                return heights[x + y * Width] / 255.0F;
            }

            return 0;
        }

        public int GetType(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                return types[x + y * Width];
            }

            return 0;
        }

        public void SetHeight(int x, int y, float height)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                height = Mathf.Clamp01(height);

                heights[x + y * Width] = (byte)(height * 255);
            }
        }

        public void SetBlockType(int x, int y, int type)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                types[x + y * Width] = (byte)type;
            }
        }

        public void Save(string path)
        {
            using (Stream writer = new StreamWriter(path).BaseStream)
            {
                writer.Write(heights, 0, heights.Length);
                writer.Write(types, 0, types.Length);
            }
        }

        public void SaveAsImage(string path)
        {
            Texture2D texture2D = new Texture2D(Width, Length);

            for(int index = 0; index < heights.Length; index++)
            {
                float gray = heights[index] / 255.0F;

                texture2D.SetPixel(index % Width, index / Width, new Color(gray, gray, gray));
            }

            texture2D.Apply();

            File.WriteAllBytes(path, texture2D.EncodeToPNG());
        }
    }
}
