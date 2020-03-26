using CustomAnimationSystem;
using System.Collections.Generic;

public interface IAnimationReader
{
    bool Connected { get; }

    void Connect();
    void Disconnect();
    List<Point> Read();
}