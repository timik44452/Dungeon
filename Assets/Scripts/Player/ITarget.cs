using UnityEngine;

/// <summary>
/// Important: use SceneUtility Create object method
/// </summary>
public interface ITarget
{
    int TypeIdentifier { get; }

    Transform transform { get; }
    GameObject gameObject { get; }
}
