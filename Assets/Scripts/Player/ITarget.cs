using UnityEngine;

/// <summary>
/// Important: use SceneUtility Create object method
/// </summary>
public interface ITarget
{
    Transform transform { get; }
    GameObject gameObject { get; }
}
