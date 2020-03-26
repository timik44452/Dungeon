using System;
using System.Net.Sockets;

using UnityEngine;
using CustomAnimationSystem;
using System.Collections.Generic;

public class AnimationReader : IAnimationReader
{
    public bool Connected
    {
        get => socket != null && socket.Connected;
    }

    public int Port
    {
        get;
    }

    private Socket socket;

    public AnimationReader(int port)
    {
        Port = port;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Connect()
    {
        socket.Connect("127.0.0.1", Port);
    }

    public void Disconnect()
    {
        socket.Disconnect(true);
    }

    public List<Point> Read()
    {
        if (socket != null && socket.Connected)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[socket.Available];

                socket.Receive(buffer);

                int index = 0;

                List<Point> points = new List<Point>();

                while (index < buffer.Length)
                {
                    int count = buffer[index];

                    float x = BitConverter.ToSingle(buffer, index + 1);
                    float y = BitConverter.ToSingle(buffer, index + 5);
                    float z = BitConverter.ToSingle(buffer, index + 9);

                    string key = System.Text.Encoding.Default.GetString(buffer, index + 13, count - 13);

                    if (points.Find(point => point.key == key) == null)
                    {
                        points.Add(new Point()
                        {
                            key = key,
                            localPosition = new Vector3(x, y, z),
                            localRotation = Vector3.zero,
                            localScale = Vector3.one
                        });
                    }

                    index += count;
                }

                return points;
            }
        }

        return new List<Point>();
    }
}
