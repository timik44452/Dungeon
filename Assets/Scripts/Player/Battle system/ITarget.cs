using UnityEngine;

public interface ITarget
{
    Transform transform { get; }
    GameObject gameObject { get; }
}
